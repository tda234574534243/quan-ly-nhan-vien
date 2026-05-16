using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DTO;
namespace DAL
{
    public class DAL_PHANLOAITK : KetNoi
    {
        public DataTable getPhanLoaiTK()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM PHANLOAITAIKHOAN", connection);
            DataTable dtPHANLOAITK = new DataTable();
            da.Fill(dtPHANLOAITK);
            return dtPHANLOAITK;
        }

        public bool ThemPhanLoaiTK(DTO_PHANLOAITK pltk)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                string sql = "INSERT INTO PHANLOAITAIKHOAN(TENLOAITAIKHOAN,QUYENHAN) VALUES (@ten, @quyen)";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@ten", pltk.TENLOAITK ?? string.Empty);
                    cmd.Parameters.AddWithValue("@quyen", pltk.QUYENHAN ?? string.Empty);
                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        using (SqlCommand a = new SqlCommand("EXEC dbo.usp_AuditLog_Add @EventType, @Username, @Target, @Details", connection))
                        {
                            a.Parameters.AddWithValue("@EventType", "RoleCreate");
                            a.Parameters.AddWithValue("@Username", string.Empty);
                            a.Parameters.AddWithValue("@Target", pltk.TENLOAITK ?? string.Empty);
                            a.Parameters.AddWithValue("@Details", "Role created");
                            a.ExecuteNonQuery();
                        }
                    }
                    return rows > 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }

        public bool SuaPhanLoaiTK(DTO_PHANLOAITK pltk)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                string sql = "UPDATE PHANLOAITAIKHOAN SET TENLOAITAIKHOAN=@ten, QUYENHAN=@quyen WHERE MALOAITK = @maloaitk";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@ten", pltk.TENLOAITK ?? string.Empty);
                    cmd.Parameters.AddWithValue("@quyen", pltk.QUYENHAN ?? string.Empty);
                    cmd.Parameters.AddWithValue("@maloaitk", pltk.MALOAITK);
                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        using (SqlCommand a = new SqlCommand("EXEC dbo.usp_AuditLog_Add @EventType, @Username, @Target, @Details", connection))
                        {
                            a.Parameters.AddWithValue("@EventType", "RoleUpdate");
                            a.Parameters.AddWithValue("@Username", string.Empty);
                            a.Parameters.AddWithValue("@Target", pltk.TENLOAITK ?? string.Empty);
                            a.Parameters.AddWithValue("@Details", "Role updated");
                            a.ExecuteNonQuery();
                        }
                    }
                    return rows > 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }

        public bool XoaPhanLoaiTK(int maltk)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                string sql = "DELETE FROM PHANLOAITAIKHOAN WHERE MALOAITK = @maloaitk";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@maloaitk", maltk);
                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        using (SqlCommand a = new SqlCommand("EXEC dbo.usp_AuditLog_Add @EventType, @Username, @Target, @Details", connection))
                        {
                            a.Parameters.AddWithValue("@EventType", "RoleDelete");
                            a.Parameters.AddWithValue("@Username", string.Empty);
                            a.Parameters.AddWithValue("@Target", maltk.ToString());
                            a.Parameters.AddWithValue("@Details", "Role deleted");
                            a.ExecuteNonQuery();
                        }
                    }
                    return rows > 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }

        // Assign role to user
        public bool AssignRoleToUser(int matk, int maloaitk)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand("EXEC dbo.usp_UserRole_Assign @matk, @maloaitk", connection))
                {
                    cmd.Parameters.AddWithValue("@matk", matk);
                    cmd.Parameters.AddWithValue("@maloaitk", maloaitk);
                    int r = cmd.ExecuteNonQuery();
                    if (r >= 0)
                    {
                        using (SqlCommand a = new SqlCommand("EXEC dbo.usp_AuditLog_Add @EventType, @Username, @Target, @Details", connection))
                        {
                            a.Parameters.AddWithValue("@EventType", "UserRoleAssign");
                            a.Parameters.AddWithValue("@Username", string.Empty);
                            a.Parameters.AddWithValue("@Target", matk.ToString() + ":" + maloaitk.ToString());
                            a.Parameters.AddWithValue("@Details", "Role assigned to user");
                            a.ExecuteNonQuery();
                        }
                    }
                    return r >= 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }

        public bool RevokeRoleFromUser(int matk, int maloaitk)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand("EXEC dbo.usp_UserRole_Revoke @matk, @maloaitk", connection))
                {
                    cmd.Parameters.AddWithValue("@matk", matk);
                    cmd.Parameters.AddWithValue("@maloaitk", maloaitk);
                    int r = cmd.ExecuteNonQuery();
                    if (r >= 0)
                    {
                        using (SqlCommand a = new SqlCommand("EXEC dbo.usp_AuditLog_Add @EventType, @Username, @Target, @Details", connection))
                        {
                            a.Parameters.AddWithValue("@EventType", "UserRoleRevoke");
                            a.Parameters.AddWithValue("@Username", string.Empty);
                            a.Parameters.AddWithValue("@Target", matk.ToString() + ":" + maloaitk.ToString());
                            a.Parameters.AddWithValue("@Details", "Role revoked from user");
                            a.ExecuteNonQuery();
                        }
                    }
                    return r >= 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }
    }
}
