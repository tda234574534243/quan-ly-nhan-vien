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
                string salted = h.CreateSaltedHash(tk._MATKHAU);

                string sql = "INSERT INTO TAIKHOAN(MALOAITK,TENCHUTAIKHOAN, TENDANGNHAP, MATKHAU) VALUES (@maloaitk, @tenchu, @tendn, @matkhau)";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@maloaitk", tk._MALOAITK);
                    cmd.Parameters.AddWithValue("@tenchu", tk._TENCHUTAIKHOAN ?? string.Empty);
                    cmd.Parameters.AddWithValue("@tendn", tk._TENDANGNHAP ?? string.Empty);
                    cmd.Parameters.AddWithValue("@matkhau", salted ?? string.Empty);

                    int rows = cmd.ExecuteNonQuery();
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
                string salted = h.CreateSaltedHash(tk._MATKHAU);

                string sql = "UPDATE TAIKHOAN SET MALOAITK=@maloaitk, TENCHUTAIKHOAN=@tenchu, TENDANGNHAP=@tendn, MATKHAU=@matkhau WHERE MATK = @matk";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@maloaitk", tk._MALOAITK);
                    cmd.Parameters.AddWithValue("@tenchu", tk._TENCHUTAIKHOAN ?? string.Empty);
                    cmd.Parameters.AddWithValue("@tendn", tk._TENDANGNHAP ?? string.Empty);
                    cmd.Parameters.AddWithValue("@matkhau", salted ?? string.Empty);
                    cmd.Parameters.AddWithValue("@matk", tk._MATK);

                    int rows = cmd.ExecuteNonQuery();
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
                string sql = "SELECT MATK, MALOAITK, TENCHUTAIKHOAN, TENDANGNHAP, MATKHAU FROM TAIKHOAN WHERE TENDANGNHAP = @tendn";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@tendn", tk._TENDANGNHAP ?? string.Empty);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int matk = Convert.ToInt32(reader[0]);
                            int maloaitk = Convert.ToInt32(reader[1]);
                            string tenchu = reader[2].ToString();
                            string stored = reader[4].ToString();

                            var h = new Hash256();

                            // New salted format contains ':' separator
                            if (stored.Contains(":"))
                            {
                                if (h.Verify(tk._MATKHAU, stored))
                                {
                                    tk._MATK = matk;
                                    tk._MALOAITK = maloaitk;
                                    tk._TENCHUTAIKHOAN = tenchu;
                                    return true;
                                }
                                return false;
                            }

                            // Legacy unsalted hex SHA256: compute hash and compare
                            string inputHash;
                            using (var sha = SHA256.Create())
                            {
                                var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(tk._MATKHAU ?? string.Empty));
                                var sb2 = new StringBuilder();
                                for (int i = 0; i < bytes.Length; i++)
                                    sb2.Append(bytes[i].ToString("x2"));
                                inputHash = sb2.ToString();
                            }

                            if (StringComparer.OrdinalIgnoreCase.Compare(inputHash, stored) == 0)
                            {
                                // matched legacy hash; prepare to migrate to salted storage
                                string newSalted = h.CreateSaltedHash(tk._MATKHAU);
                                // assign DTO fields
                                tk._MATK = matk;
                                tk._MALOAITK = maloaitk;
                                tk._TENCHUTAIKHOAN = tenchu;

                                // need to update DB to new salted hash after reader is closed
                                // store migration values in locals and perform update after reader using block
                                reader.Close();

                                string updSql = "UPDATE TAIKHOAN SET MATKHAU = @matkhau WHERE MATK = @matk";
                                using (SqlCommand upd = new SqlCommand(updSql, connection))
                                {
                                    upd.Parameters.AddWithValue("@matkhau", newSalted ?? string.Empty);
                                    upd.Parameters.AddWithValue("@matk", matk);
                                    upd.ExecuteNonQuery();
                                }

                                return true;
                            }
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

        public bool KiemTraTonTai(DTO_TAIKHOAN tk)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            string sql = string.Format("SELECT * FROM TAIKHOAN WHERE TENDANGNHAP='{0}' AND TENCHUTAIKHOAN=N'{1}'", tk._TENDANGNHAP, tk._TENCHUTAIKHOAN);
            SqlCommand cmd = new SqlCommand(sql, connection);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read() == true)
            {
                tk._MATK = Convert.ToInt32(reader[0].ToString());
                tk._MALOAITK = Convert.ToInt32(reader[1].ToString());
                if (!reader.IsClosed)
                    reader.Close();
                return true;
            }
            if (!reader.IsClosed)
                reader.Close();
            return false;

        }
        public bool KiemTraTonTai(string tenDangNhap)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            string sql = string.Format("SELECT * FROM TAIKHOAN WHERE TENDANGNHAP='{0}' ", tenDangNhap);
            SqlCommand cmd = new SqlCommand(sql, connection);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read() == true)
            {
                if (!reader.IsClosed)
                    reader.Close();
                return true;

            }
            if (!reader.IsClosed)
                reader.Close();
            return false;

        }

        public bool LayMatKhau(DTO_TAIKHOAN tk)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                string sql = "SELECT MATKHAU FROM TAIKHOAN WHERE MATK = @matk";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@matk", tk._MATK);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            tk._MATKHAU = reader[0].ToString();
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
    }
}
