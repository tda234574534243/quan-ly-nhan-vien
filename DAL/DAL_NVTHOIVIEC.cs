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
    public class DAL_NVTHOIVIEC : KetNoi
    {

        public DataTable getNVThoiViec()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT MANV 'Mã nhân viên',HOTEN 'Họ tên',CMND_CCCD 'CMND-CCCD',FORMAT(NGAYTHOIVIEC, 'MM/dd/yyyy') 'Ngày thôi việc',LYDO 'Lý do' FROM NVTHOIVIEC", connection);
            DataTable dtNVTHOIVIEC = new DataTable();
            da.Fill(dtNVTHOIVIEC);
            return dtNVTHOIVIEC;
        }
        public bool ThemNVThoiViec(DTO_NVTHOIVIEC nvThoiViec)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                string sql = "INSERT INTO NVTHOIVIEC(MANV, HOTEN, CMND_CCCD, NGAYTHOIVIEC, LYDO) VALUES(@manv,@hoten,@cmnd,@ngay,@lydo)";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@manv", nvThoiViec.Manv);
                    cmd.Parameters.AddWithValue("@hoten", nvThoiViec.Hoten ?? string.Empty);
                    cmd.Parameters.AddWithValue("@cmnd", nvThoiViec.Cmnd_cccd ?? string.Empty);
                    cmd.Parameters.AddWithValue("@ngay", (object)nvThoiViec.Ngaythoiviec ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@lydo", nvThoiViec.Lydo ?? string.Empty);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }
        /*
MANV INT PRIMARY KEY,
HOTEN NVARCHAR(70),
CMND_CCCD VARCHAR(12),
NGAYTHOIVIEC DATETIME,
LYDO NVARCHAR(50)
 */
        public bool SuaNVThoiViec(DTO_NVTHOIVIEC nvThoiViec)
        {
            if (connection.State != ConnectionState.Open) connection.Open();
            try
            {
                string sql = "UPDATE NVTHOIVIEC SET HOTEN=@hoten, CMND_CCCD=@cmnd, NGAYTHOIVIEC=@ngay, LYDO=@lydo WHERE MANV=@manv";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@hoten", nvThoiViec.Hoten ?? string.Empty);
                    cmd.Parameters.AddWithValue("@cmnd", nvThoiViec.Cmnd_cccd ?? string.Empty);
                    cmd.Parameters.AddWithValue("@ngay", (object)nvThoiViec.Ngaythoiviec ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@lydo", nvThoiViec.Lydo ?? string.Empty);
                    cmd.Parameters.AddWithValue("@manv", nvThoiViec.Manv);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }

        public bool XoaNVThoiViec(int manv)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("DELETE FROM NVTHOIVIEC WHERE MANV = @manv", connection))
                {
                    cmd.Parameters.AddWithValue("@manv", manv);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }

        public int SoLuongNhanVienNghiViec(int thang, int nam)
        {
            int n = 0;
            if (connection.State != ConnectionState.Open) connection.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(1) FROM NVTHOIVIEC WHERE MONTH(NGAYTHOIVIEC)=@thang AND YEAR(NGAYTHOIVIEC)=@nam", connection))
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
    }
}
