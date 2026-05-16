using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using DTO;
namespace DAL
{
    public class DAL_TAIKHOAN : KetNoi
    {
        private void Audit(string eventType, string username, string target, string details)
        {
            try
            {
                // Use centralized audit helper to ensure all audits go through stored procedure
                var a = new DAL_Audit();
                a.WriteAudit(eventType ?? string.Empty, username ?? string.Empty, target ?? string.Empty, details ?? string.Empty);
            }
            catch { /* swallow audit errors to not break auth flow */ }
        }
        public DataTable getTaiKhoan()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT MATK 'Mã tài khoản', MALOAITK 'Mã loại tài khoản', TENCHUTAIKHOAN 'Tên chủ tài khoản', TENDANGNHAP 'Tên đăng nhập', MATKHAU 'Mật khẩu mã hóa' FROM TAIKHOAN", connection);
            DataTable dtTAIKHOAN = new DataTable();
            da.Fill(dtTAIKHOAN);
            return dtTAIKHOAN;
        }
        public bool ThemTaikhoan(DTO_TAIKHOAN tk)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                var h = new Hash256();
                string salted = h.CreateHash(tk._MATKHAU);

                string sql = "INSERT INTO TAIKHOAN(MALOAITK,TENCHUTAIKHOAN, TENDANGNHAP, MATKHAU) VALUES (@maloaitk, @tenchu, @tendn, @matkhau)";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@maloaitk", tk._MALOAITK);
                    cmd.Parameters.AddWithValue("@tenchu", tk._TENCHUTAIKHOAN ?? string.Empty);
                    cmd.Parameters.AddWithValue("@tendn", tk._TENDANGNHAP ?? string.Empty);
                    cmd.Parameters.AddWithValue("@matkhau", salted ?? string.Empty);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0) Audit("UserCreate", tk._TENDANGNHAP, null, "User created");
                    return rows > 0;
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        public bool SuaTaiKhoan(DTO_TAIKHOAN tk)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                var h = new Hash256();
                string salted = h.CreateHash(tk._MATKHAU);

                string sql = "UPDATE TAIKHOAN SET MALOAITK=@maloaitk, TENCHUTAIKHOAN=@tenchu, TENDANGNHAP=@tendn, MATKHAU=@matkhau WHERE MATK = @matk";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@maloaitk", tk._MALOAITK);
                    cmd.Parameters.AddWithValue("@tenchu", tk._TENCHUTAIKHOAN ?? string.Empty);
                    cmd.Parameters.AddWithValue("@tendn", tk._TENDANGNHAP ?? string.Empty);
                    cmd.Parameters.AddWithValue("@matkhau", salted ?? string.Empty);
                    cmd.Parameters.AddWithValue("@matk", tk._MATK);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0) Audit("UserUpdate", tk._TENDANGNHAP, tk._MATK.ToString(), "User updated");
                    return rows > 0;
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        public bool XoaTaiKhoan(int matk)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                string sql = "DELETE FROM TAIKHOAN WHERE MATK = @matk";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@matk", matk);
                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0) Audit("UserDelete", null, matk.ToString(), "User deleted");
                    return rows > 0;
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }


        public bool KiemTraTaiKhoan(DTO_TAIKHOAN tk)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                // Load account including lockout info via stored procedure wrapper (app schema)
                using (SqlCommand cmd = new SqlCommand("app.usp_User_GetByUsername", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TENDANGNHAP", tk._TENDANGNHAP ?? string.Empty);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read()) return false;

                        int matk = Convert.ToInt32(reader[0]);
                        int maloaitk = Convert.ToInt32(reader[1]);
                        string tenchu = reader[2].ToString();
                        string stored = reader[4].ToString();
                        int failed = reader.IsDBNull(5) ? 0 : Convert.ToInt32(reader[5]);
                        DateTime? lockoutUntil = reader.IsDBNull(6) ? (DateTime?)null : Convert.ToDateTime(reader[6]);

                        // Check lockout
                        if (lockoutUntil.HasValue && lockoutUntil.Value > DateTime.UtcNow)
                        {
                            Audit("LoginBlocked", tk._TENDANGNHAP, matk.ToString(), "Account locked");
                            return false;
                        }

                        var h = new Hash256();
                        // verify PBKDF2
                        bool verified = stored.StartsWith("pbkdf2$", StringComparison.Ordinal) ? h.Verify(tk._MATKHAU, stored) : false;

                        if (!verified && !stored.StartsWith("pbkdf2$", StringComparison.Ordinal))
                        {
                            // legacy unsalted hex SHA256: verify and migrate to pbkdf2
                            if (h.VerifyLegacySha256(tk._MATKHAU, stored))
                            {
                                verified = true;
                                // migrate
                                string newHash = h.CreateHash(tk._MATKHAU);
                                reader.Close();
                                using (SqlCommand upd = new SqlCommand("UPDATE TAIKHOAN SET MATKHAU = @matkhau WHERE MATK = @matk", connection))
                                {
                                    upd.Parameters.AddWithValue("@matkhau", newHash ?? string.Empty);
                                    upd.Parameters.AddWithValue("@matk", matk);
                                    upd.ExecuteNonQuery();
                                }
                                Audit("LoginSuccess_Migrated", tk._TENDANGNHAP, matk.ToString(), "Login successful; password migrated to PBKDF2");
                            }
                        }

                        if (verified)
                        {
                            // reset failed counter
                            reader.Close();
                            using (SqlCommand upd = new SqlCommand("UPDATE TAIKHOAN SET FailedLoginCount = 0, LockoutUntil = NULL WHERE MATK = @matk", connection))
                            {
                                upd.Parameters.AddWithValue("@matk", matk);
                                upd.ExecuteNonQuery();
                            }
                            tk._MATK = matk;
                            tk._MALOAITK = maloaitk;
                            tk._TENCHUTAIKHOAN = tenchu;
                            Audit("LoginSuccess", tk._TENDANGNHAP, matk.ToString(), "Login successful");
                            return true;
                        }

                        // Failed login: increment counter and possibly lock
                        reader.Close();
                        int threshold = 5;
                        int minutes = 15;
                        // read THAMSO if available
                        try
                        {
                            using (SqlCommand getTs = new SqlCommand("SELECT GIATRI FROM THAMSO WHERE MATHAMSO = @m", connection))
                            {
                                getTs.Parameters.AddWithValue("@m", "TS06");
                                var r = getTs.ExecuteScalar();
                                if (r != null) threshold = Convert.ToInt32(r);
                                getTs.Parameters.Clear();
                                getTs.Parameters.AddWithValue("@m", "TS07");
                                var r2 = getTs.ExecuteScalar();
                                if (r2 != null) minutes = Convert.ToInt32(r2);
                            }
                        }
                        catch { }

                        using (SqlCommand inc = new SqlCommand("UPDATE TAIKHOAN SET FailedLoginCount = FailedLoginCount + 1 WHERE MATK = @matk", connection))
                        {
                            inc.Parameters.AddWithValue("@matk", matk);
                            inc.ExecuteNonQuery();
                        }

                        // get updated count
                        int newFailed = 0;
                        using (SqlCommand getf = new SqlCommand("SELECT FailedLoginCount FROM TAIKHOAN WHERE MATK = @matk", connection))
                        {
                            getf.Parameters.AddWithValue("@matk", matk);
                            var rf = getf.ExecuteScalar();
                            if (rf != null) newFailed = Convert.ToInt32(rf);
                        }

                        if (newFailed >= threshold)
                        {
                            DateTime lockUntil = DateTime.UtcNow.AddMinutes(minutes);
                            using (SqlCommand lockc = new SqlCommand("UPDATE TAIKHOAN SET LockoutUntil = @lockuntil WHERE MATK = @matk", connection))
                            {
                                lockc.Parameters.AddWithValue("@lockuntil", lockUntil);
                                lockc.Parameters.AddWithValue("@matk", matk);
                                lockc.ExecuteNonQuery();
                            }
                            Audit("LoginLocked", tk._TENDANGNHAP, matk.ToString(), "Account locked due to failed logins");
                        }

                        Audit("LoginFailure", tk._TENDANGNHAP, null, "Login failed: bad credentials");
                        return false;
                    }
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        public bool KiemTraTonTai(DTO_TAIKHOAN tk)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                string sql = "SELECT MATK, MALOAITK FROM TAIKHOAN WHERE TENDANGNHAP = @tendn AND TENCHUTAIKHOAN = @tenchu";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@tendn", tk._TENDANGNHAP ?? string.Empty);
                    cmd.Parameters.AddWithValue("@tenchu", tk._TENCHUTAIKHOAN ?? string.Empty);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            tk._MATK = Convert.ToInt32(reader[0].ToString());
                            tk._MALOAITK = Convert.ToInt32(reader[1].ToString());
                            return true;
                        }
                        return false;
                    }
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }

        }
        public bool KiemTraTonTai(string tenDangNhap)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                string sql = "SELECT 1 FROM TAIKHOAN WHERE TENDANGNHAP = @tendn";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@tendn", tenDangNhap ?? string.Empty);
                    var res = cmd.ExecuteScalar();
                    return res != null;
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }

        }

        public bool LayMatKhau(DTO_TAIKHOAN tk)
        {
            // For security, do not return password hash to callers. This method is deprecated.
            return false;
        }
    }
}
