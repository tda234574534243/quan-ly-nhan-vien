using DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DAL
{
    public class DAL_BOPHAN : KetNoi
    {
        public DataTable getBoPhan()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT MABP 'Mã bộ phận', TENBOPHAN 'Tên bộ phận', FORMAT(NGAYTHANHLAP, 'MM/dd/yyyy') 'Ngày thành lập', GHICHU 'Ghi chú' FROM BOPHAN", connection);
            DataTable dtBOPHAN = new DataTable();
            da.Fill(dtBOPHAN);
            return dtBOPHAN;
        }
        public bool ThemBoPhan(DTO_BOPHAN boPhan)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                string sql = "INSERT INTO BOPHAN(MABP, TENBOPHAN, NGAYTHANHLAP, GHICHU) VALUES(@mabp, @ten, @ngay, @ghichu)";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@mabp", boPhan.Mabp ?? string.Empty);
                    cmd.Parameters.AddWithValue("@ten", boPhan.Tenbophan ?? string.Empty);
                    cmd.Parameters.AddWithValue("@ngay", boPhan.Ngaythanhlap == default(DateTime) ? (object)DBNull.Value : (object)boPhan.Ngaythanhlap);
                    cmd.Parameters.AddWithValue("@ghichu", boPhan.Ghichu ?? string.Empty);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }
        /*
    MABP VARCHAR(8) PRIMARY KEY,
	TENBOPHAN NVARCHAR(20),
	NGAYTHANHLAP DATETIME,
	GHICHU NVARCHAR(70)
 */
        public bool SuaBoPhan(DTO_BOPHAN boPhan)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                string sql = "UPDATE BOPHAN SET TENBOPHAN=@ten, NGAYTHANHLAP=@ngay, GHICHU=@ghichu WHERE MABP = @mabp";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@ten", boPhan.Tenbophan ?? string.Empty);
                    cmd.Parameters.AddWithValue("@ngay", boPhan.Ngaythanhlap == default(DateTime) ? (object)DBNull.Value : (object)boPhan.Ngaythanhlap);
                    cmd.Parameters.AddWithValue("@ghichu", boPhan.Ghichu ?? string.Empty);
                    cmd.Parameters.AddWithValue("@mabp", boPhan.Mabp ?? string.Empty);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }

        public bool XoaBoPhan(string mabp)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand("DELETE FROM BOPHAN WHERE MABP = @mabp", connection))
                {
                    cmd.Parameters.AddWithValue("@mabp", mabp ?? string.Empty);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }


        public string TimKiemTheoTenBoPhan(string tenBP)
        {
            string maBP = string.Empty;
            CheckConnection();
            using (SqlCommand cmd = new SqlCommand("SELECT MABP FROM BOPHAN WHERE TENBOPHAN = @ten", connection))
            {
                cmd.Parameters.AddWithValue("@ten", tenBP ?? string.Empty);
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read()) maBP = sdr["MABP"].ToString();
                }
            }
            connection.Close();
            return maBP;
        }

        public string TimKiemTheoMaBoPhan(string maBP)
        {
            string tenBP = string.Empty;
            CheckConnection();
            using (SqlCommand cmd = new SqlCommand("SELECT TENBOPHAN FROM BOPHAN WHERE MABP = @mabp", connection))
            {
                cmd.Parameters.AddWithValue("@mabp", maBP ?? string.Empty);
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read()) tenBP = sdr["TENBOPHAN"].ToString();
                }
            }
            connection.Close();
            return tenBP;
        }

        public List<string> TongHopTenBoPhan()
        {
            List<string> listTenBoPhan = new List<string>();
            CheckConnection();
            using (SqlCommand cmd = new SqlCommand("SELECT TENBOPHAN FROM BOPHAN", connection))
            using (SqlDataReader sdr = cmd.ExecuteReader())
            {
                while (sdr.Read()) listTenBoPhan.Add(sdr[0].ToString());
            }
            connection.Close();
            return listTenBoPhan;
        }
        public List<string> TongHopMaBoPhan()
        {
            List<string> listMaBoPhan = new List<string>();
            CheckConnection();
            using (SqlCommand cmd = new SqlCommand("SELECT MABP FROM BOPHAN", connection))
            using (SqlDataReader sdr = cmd.ExecuteReader())
            {
                while (sdr.Read()) listMaBoPhan.Add(sdr[0].ToString());
            }
            connection.Close();
            return listMaBoPhan;
        }
    }
}
