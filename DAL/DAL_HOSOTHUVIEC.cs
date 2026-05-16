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
    public class DAL_HOSOTHUVIEC : KetNoi
    {

        public DataTable getHoSoThuViec()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT MANVTV 'Mã nhân viên', HOTEN 'Họ tên',FORMAT(NGAYSINH, 'MM/dd/yyyy') 'Ngày sinh',GIOITINH 'Giới tính',CMND_CCCD 'CMND-CCCD', NOICAP 'Nơi cấp',VITRITHUVIEC 'Vị trí thử việc', FORMAT(NGAYTV, 'MM/dd/yyyy') 'Ngày thử việc', SOTHANGTV 'Số tháng thử việc', SDT 'Số điện thoại', HOCVAN 'Học vấn', GHICHU 'Ghi chú' FROM HOSOTHUVIEC", connection);
            DataTable dtHOSOTHUVIEC = new DataTable();
            da.Fill(dtHOSOTHUVIEC);
            return dtHOSOTHUVIEC;
        }
        public bool ThemHoSoThuViec(DTO_HOSOTHUVIEC hoSoThuViec)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                string sql = "INSERT INTO HOSOTHUVIEC(HOTEN, NGAYSINH, GIOITINH, CMND_CCCD, NOICAP, VITRITHUVIEC, NGAYTV, SOTHANGTV, SDT, HOCVAN, GHICHU) VALUES(@hoten,@ngaysinh,@gioitinh,@cmnd,@noicap,@vitri,@ngaytv,@sothang,@sdt,@hocvan,@ghichu)";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@hoten", hoSoThuViec.Hoten ?? string.Empty);
                    cmd.Parameters.AddWithValue("@ngaysinh", (object)hoSoThuViec.Ngaysinh ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@gioitinh", hoSoThuViec.Gioitinh ?? string.Empty);
                    cmd.Parameters.AddWithValue("@cmnd", hoSoThuViec.Cmnd_cccd ?? string.Empty);
                    cmd.Parameters.AddWithValue("@noicap", hoSoThuViec.Noicap ?? string.Empty);
                    cmd.Parameters.AddWithValue("@vitri", hoSoThuViec.Vitrithuviec ?? string.Empty);
                    cmd.Parameters.AddWithValue("@ngaytv", (object)hoSoThuViec.Ngaytv ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@sothang", hoSoThuViec.Sothangtv);
                    cmd.Parameters.AddWithValue("@sdt", hoSoThuViec.Sdt ?? string.Empty);
                    cmd.Parameters.AddWithValue("@hocvan", hoSoThuViec.Hocvan ?? string.Empty);
                    cmd.Parameters.AddWithValue("@ghichu", hoSoThuViec.Ghichu ?? string.Empty);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }
        /*
MANVTV INT IDENTITY(1,1) PRIMARY KEY,
	HOTEN NVARCHAR(70),
	NGAYSINH DATETIME,
	GIOITINH NVARCHAR(3),
	CMND_CCCD VARCHAR(12),
	NOICAP NVARCHAR(20),
	VITRITHUVIEC NVARCHAR(25),
	NGAYTV DATETIME,
	SOTHANGTV INT,
	SDT VARCHAR(10),
	HOCVAN NVARCHAR(20),
	GHICHU NVARCHAR(60)
 */
        public bool SuaHoSoThuViec(DTO_HOSOTHUVIEC hoSoThuViec)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                string sql = "UPDATE HOSOTHUVIEC SET HOTEN=@hoten, NGAYSINH=@ngaysinh, GIOITINH=@gioitinh, CMND_CCCD=@cmnd, NOICAP=@noicap, VITRITHUVIEC=@vitri, NGAYTV=@ngaytv, SOTHANGTV=@sothang, SDT=@sdt, HOCVAN=@hocvan, GHICHU=@ghichu WHERE MANVTV=@manvtv";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@hoten", hoSoThuViec.Hoten ?? string.Empty);
                    cmd.Parameters.AddWithValue("@ngaysinh", (object)hoSoThuViec.Ngaysinh ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@gioitinh", hoSoThuViec.Gioitinh ?? string.Empty);
                    cmd.Parameters.AddWithValue("@cmnd", hoSoThuViec.Cmnd_cccd ?? string.Empty);
                    cmd.Parameters.AddWithValue("@noicap", hoSoThuViec.Noicap ?? string.Empty);
                    cmd.Parameters.AddWithValue("@vitri", hoSoThuViec.Vitrithuviec ?? string.Empty);
                    cmd.Parameters.AddWithValue("@ngaytv", (object)hoSoThuViec.Ngaytv ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@sothang", hoSoThuViec.Sothangtv);
                    cmd.Parameters.AddWithValue("@sdt", hoSoThuViec.Sdt ?? string.Empty);
                    cmd.Parameters.AddWithValue("@hocvan", hoSoThuViec.Hocvan ?? string.Empty);
                    cmd.Parameters.AddWithValue("@ghichu", hoSoThuViec.Ghichu ?? string.Empty);
                    cmd.Parameters.AddWithValue("@manvtv", hoSoThuViec.Manvtv);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }

        public bool XoaHoSoThuViec(int manvtv)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand("DELETE FROM HOSOTHUVIEC WHERE MANVTV = @manvtv", connection))
                {
                    cmd.Parameters.AddWithValue("@manvtv", manvtv);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }

        public int SoLuongNhanVienThuViec(int thang, int nam)
        {
            int n = 0;
            if (connection.State != ConnectionState.Open) connection.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(1) FROM HOSOTHUVIEC WHERE MONTH(NGAYTV)=@thang AND YEAR(NGAYTV)=@nam", connection))
                {
                    cmd.Parameters.AddWithValue("@thang", thang);
                    cmd.Parameters.AddWithValue("@nam", nam);
                    object res = cmd.ExecuteScalar();
                    if (res != null) n = Convert.ToInt32(res);
                }
                return n;
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }

        public List<string> TongHopMaNhanVien()
        {
            List<string> listMaNhanVien = new List<string>();
            CheckConnection();
            using (SqlCommand cmd = new SqlCommand("SELECT MANVTV FROM HOSOTHUVIEC", connection))
            using (SqlDataReader sdr = cmd.ExecuteReader())
            {
                while (sdr.Read()) listMaNhanVien.Add(sdr[0].ToString());
            }
            connection.Close();
            return listMaNhanVien;
        }
    }
}
