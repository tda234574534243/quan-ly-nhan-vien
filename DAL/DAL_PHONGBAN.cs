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
    public class DAL_PHONGBAN : KetNoi
    {

        public DataTable getPhongBan()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT MAPHONG 'Mã phòng', MABP 'Mã bộ phận', TENPHONG 'Tên phòng', FORMAT(NGAYTHANHLAP, 'MM/dd/yyyy') 'Ngày thành lập', GHICHU 'Ghi chú' FROM PHONGBAN", connection);
            DataTable dtPHONGBAN = new DataTable();
            da.Fill(dtPHONGBAN);
            return dtPHONGBAN;
        }
        public bool ThemPhongBan(DTO_PHONGBAN phongBan)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                string sql = "INSERT INTO PHONGBAN(MAPHONG, MABP, TENPHONG, NGAYTHANHLAP, GHICHU) VALUES(@maphong, @mabp, @tenphong, @ngay, @ghichu)";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@maphong", phongBan.Maphong ?? string.Empty);
                    cmd.Parameters.AddWithValue("@mabp", phongBan.Mabp ?? string.Empty);
                    cmd.Parameters.AddWithValue("@tenphong", phongBan.Tenphong ?? string.Empty);
                    cmd.Parameters.AddWithValue("@ngay", phongBan.Ngaythanhlap == default(DateTime) ? (object)DBNull.Value : (object)phongBan.Ngaythanhlap);
                    cmd.Parameters.AddWithValue("@ghichu", phongBan.Ghichu ?? string.Empty);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }
        /*
	MAPHONG VARCHAR(6) PRIMARY KEY,
	MABP VARCHAR(8),
	TENPHONG NVARCHAR(20),
	NGAYTHANHLAP DATETIME,
	GHICHU NVARCHAR(70)
 */
        public bool SuaPhongBan(DTO_PHONGBAN phongBan)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                string sql = "UPDATE PHONGBAN SET MABP=@mabp, TENPHONG=@tenphong, NGAYTHANHLAP=@ngay, GHICHU=@ghichu WHERE MAPHONG=@maphong";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@mabp", phongBan.Mabp ?? string.Empty);
                    cmd.Parameters.AddWithValue("@tenphong", phongBan.Tenphong ?? string.Empty);
                    cmd.Parameters.AddWithValue("@ngay", phongBan.Ngaythanhlap == default(DateTime) ? (object)DBNull.Value : (object)phongBan.Ngaythanhlap);
                    cmd.Parameters.AddWithValue("@ghichu", phongBan.Ghichu ?? string.Empty);
                    cmd.Parameters.AddWithValue("@maphong", phongBan.Maphong ?? string.Empty);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }

        public bool XoaPhongBan(string maphong)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand("DELETE FROM PHONGBAN WHERE MAPHONG = @maphong", connection))
                {
                    cmd.Parameters.AddWithValue("@maphong", maphong ?? string.Empty);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }

        public string TimKiemMaPhongBan(string tenPhong)
        {
            string maPhong = string.Empty;
            CheckConnection();
            using (SqlCommand cmd = new SqlCommand("SELECT MAPHONG FROM PHONGBAN WHERE TENPHONG = @ten", connection))
            {
                cmd.Parameters.AddWithValue("@ten", tenPhong ?? string.Empty);
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read()) maPhong = sdr["MAPHONG"].ToString();
                }
            }
            connection.Close();
            return maPhong;
        }

        public string TimKiemTenPhongBanTheoMa(string maPhong)
        {
            string tenPhong = string.Empty;
            CheckConnection();
            using (SqlCommand cmd = new SqlCommand("SELECT TENPHONG FROM PHONGBAN WHERE MAPHONG = @maphong", connection))
            {
                cmd.Parameters.AddWithValue("@maphong", maPhong ?? string.Empty);
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read()) tenPhong = sdr["TENPHONG"].ToString();
                }
            }
            connection.Close();
            return tenPhong;
        }

        public string TimKiemBoPhanTheoPhong(string maPhong)
        {
            string tenPhong = string.Empty;
            CheckConnection();
            using (SqlCommand cmd = new SqlCommand("SELECT MABP FROM PHONGBAN WHERE MAPHONG = @maphong", connection))
            {
                cmd.Parameters.AddWithValue("@maphong", maPhong ?? string.Empty);
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                        tenPhong = sdr["MABP"].ToString();
                }
            }
            connection.Close();
            return tenPhong;
        }

        public List<string> TongHopPhongBan(string maBP)
        {
            List<string> listPhongBan = new List<string>();
            CheckConnection();
            if (maBP == "")
            {
                using (SqlCommand cmd = new SqlCommand("SELECT TENPHONG FROM PHONGBAN", connection))
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read()) listPhongBan.Add(sdr[0].ToString());
                }
            }
            else
            {
                using (SqlCommand cmd = new SqlCommand("SELECT TENPHONG FROM PHONGBAN WHERE MABP = @mabp", connection))
                {
                    cmd.Parameters.AddWithValue("@mabp", maBP ?? string.Empty);
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read()) listPhongBan.Add(sdr[0].ToString());
                    }
                }
            }
            connection.Close();
            return listPhongBan;
        }
        public List<string> TongHopMaPhongBan()
        {
            List<string> listMaPhongBan = new List<string>();
            CheckConnection();
            using (SqlCommand cmd = new SqlCommand("SELECT MAPHONG FROM PHONGBAN", connection))
            using (SqlDataReader sdr = cmd.ExecuteReader())
            {
                while (sdr.Read()) listMaPhongBan.Add(sdr[0].ToString());
            }
            connection.Close();
            return listMaPhongBan;
        }
    }
}
