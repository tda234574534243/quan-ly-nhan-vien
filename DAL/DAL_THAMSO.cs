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
    public class DAL_THAMSO : KetNoi
    {
        public DataTable getThamSo()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT MATHAMSO 'Mã tham số', TENTHAMSO 'Tên tham số', GIATRI 'Giá trị' FROM THAMSO", connection);
            DataTable dtTHAMSO = new DataTable();
            da.Fill(dtTHAMSO);
            return dtTHAMSO;
        }


        public bool SuaThamSo(DTO_THAMSO ts)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                string sql = "UPDATE THAMSO SET GIATRI = @giatri WHERE MATHAMSO = @mathamso";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@giatri", ts.Giatri);
                    cmd.Parameters.AddWithValue("@mathamso", ts.Mats ?? string.Empty);
                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        // audit
                        using (SqlCommand a = new SqlCommand("EXEC dbo.usp_AuditLog_Add @EventType, @Username, @Target, @Details", connection))
                        {
                            a.Parameters.AddWithValue("@EventType", "ParamUpdate");
                            a.Parameters.AddWithValue("@Username", string.Empty);
                            a.Parameters.AddWithValue("@Target", ts.Mats ?? string.Empty);
                            a.Parameters.AddWithValue("@Details", "Parameter updated");
                            a.ExecuteNonQuery();
                        }
                    }
                    return rows > 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }

        public double Get_soNgayNghiToiDa()
        {
            if (connection.State != ConnectionState.Open) connection.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT GIATRI FROM THAMSO WHERE MATHAMSO = @m", connection))
                {
                    cmd.Parameters.AddWithValue("@m", "TS01");
                    object res = cmd.ExecuteScalar();
                    if (res != null) return Convert.ToDouble(res);
                    return 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }
        public double Get_soNgayLamToiDa()
        {
            if (connection.State != ConnectionState.Open) connection.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT GIATRI FROM THAMSO WHERE MATHAMSO = @m", connection))
                {
                    cmd.Parameters.AddWithValue("@m", "TS02");
                    object res = cmd.ExecuteScalar();
                    if (res != null) return Convert.ToDouble(res);
                    return 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }
        public double Get_tienLamthem()
        {
            if (connection.State != ConnectionState.Open) connection.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT GIATRI FROM THAMSO WHERE MATHAMSO = @m", connection))
                {
                    cmd.Parameters.AddWithValue("@m", "TS03");
                    object res = cmd.ExecuteScalar();
                    if (res != null) return Convert.ToDouble(res);
                    return 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }
        public double Get_tiLePhat()
        {
            if (connection.State != ConnectionState.Open) connection.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT GIATRI FROM THAMSO WHERE MATHAMSO = @m", connection))
                {
                    cmd.Parameters.AddWithValue("@m", "TS04");
                    object res = cmd.ExecuteScalar();
                    if (res != null) return Convert.ToDouble(res);
                    return 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }

        public double Get_soThangNghiSinh()
        {
            if (connection.State != ConnectionState.Open) connection.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT GIATRI FROM THAMSO WHERE MATHAMSO = @m", connection))
                {
                    cmd.Parameters.AddWithValue("@m", "TS05");
                    object res = cmd.ExecuteScalar();
                    if (res != null) return Convert.ToDouble(res);
                    return 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }

    }
}
