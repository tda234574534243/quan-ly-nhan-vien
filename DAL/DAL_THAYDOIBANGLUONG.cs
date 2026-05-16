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
    public class DAL_THAYDOIBANGLUONG : KetNoi
    {

        public DataTable getThayDoiBangLuong()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT MANV 'Mã nhân viên', MALUONG 'Mã lương cũ', MALUONGMOI 'Mã lương mới', FORMAT(NGAYSUA, 'MM/dd/yyyy') 'Ngày sửa', LYDO 'Lý do' FROM THAYDOIBANGLUONG", connection);
            DataTable dtTHAYDOIBANGLUONG = new DataTable();
            da.Fill(dtTHAYDOIBANGLUONG);
            return dtTHAYDOIBANGLUONG;
        }
        public bool ThemThayDoiBangLuong(DTO_THAYDOIBANGLUONG tdbl)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                string sql = "INSERT INTO THAYDOIBANGLUONG(MANV, MALUONG, MALUONGMOI, NGAYSUA, LYDO) VALUES(@manv,@maluong,@maluongmoi,@ngaysua,@lydo)";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@manv", tdbl.Manv);
                    cmd.Parameters.AddWithValue("@maluong", tdbl.Maluong ?? string.Empty);
                    cmd.Parameters.AddWithValue("@maluongmoi", tdbl.Maluongmoi ?? string.Empty);
                    cmd.Parameters.AddWithValue("@ngaysua", (object)tdbl.Ngaysua ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@lydo", tdbl.Lydo ?? string.Empty);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }
        /*
	MANV INT,
	MALUONG VARCHAR(8),
	MALUONGMOI VARCHAR(8),
	PRIMARY KEY (MANV, MALUONG, MALUONGMOI),
	NGAYSUA DATETIME,
	LYDO NVARCHAR(70)
 */
        public bool SuaThayDoiBangLuong(DTO_THAYDOIBANGLUONG tdbl)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                string sql = "UPDATE THAYDOIBANGLUONG SET NGAYSUA=@ngaysua, LYDO=@lydo WHERE MANV=@manv AND MALUONG=@maluong AND MALUONGMOI=@maluongmoi";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@ngaysua", (object)tdbl.Ngaysua ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@lydo", tdbl.Lydo ?? string.Empty);
                    cmd.Parameters.AddWithValue("@manv", tdbl.Manv);
                    cmd.Parameters.AddWithValue("@maluong", tdbl.Maluong ?? string.Empty);
                    cmd.Parameters.AddWithValue("@maluongmoi", tdbl.Maluongmoi ?? string.Empty);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }

        public bool XoaThayDoiBangLuong(int manv,string maluong,string maluongmoi)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("DELETE FROM THAYDOIBANGLUONG WHERE MANV = @manv AND MALUONG=@maluong AND MALUONGMOI=@maluongmoi", connection))
                {
                    cmd.Parameters.AddWithValue("@manv", manv);
                    cmd.Parameters.AddWithValue("@maluong", maluong ?? string.Empty);
                    cmd.Parameters.AddWithValue("@maluongmoi", maluongmoi ?? string.Empty);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }

        public bool XoaThayDoiBangLuongCuaNhanVien(int manv)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("DELETE FROM THAYDOIBANGLUONG WHERE MANV = @manv", connection))
                {
                    cmd.Parameters.AddWithValue("@manv", manv);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }

        public bool KiemTraTonTaiThayDoiBangLuong(string maNV, string maLuong, string maLuongMoi)
        {
            if (connection.State != ConnectionState.Open) connection.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT 1 FROM THAYDOIBANGLUONG WHERE MANV=@manv AND MALUONG=@maluong AND MALUONGMOI=@maluongmoi", connection))
                {
                    cmd.Parameters.AddWithValue("@manv", maNV ?? string.Empty);
                    cmd.Parameters.AddWithValue("@maluong", maLuong ?? string.Empty);
                    cmd.Parameters.AddWithValue("@maluongmoi", maLuongMoi ?? string.Empty);
                    var res = cmd.ExecuteScalar();
                    return res != null;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }

        public bool KiemTraTonTaiThayDoiBangLuongTheoNhanVien(string maNV)
        {
            if (connection.State != ConnectionState.Open) connection.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT 1 FROM THAYDOIBANGLUONG WHERE MANV = @manv", connection))
                {
                    cmd.Parameters.AddWithValue("@manv", maNV ?? string.Empty);
                    var res = cmd.ExecuteScalar();
                    return res != null;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }

        public DataTable getThayDoiBangLuongCaNhan(string manv)
        { 
            SqlDataAdapter da = new SqlDataAdapter("SELECT MANV 'Mã nhân viên', MALUONG 'Mã lương cũ', MALUONGMOI 'Mã lương mới', FORMAT(NGAYSUA, 'MM/dd/yyyy') 'Ngày sửa', LYDO 'Lý do' FROM THAYDOIBANGLUONG WHERE MANV = @manv", connection);
            da.SelectCommand.Parameters.AddWithValue("@manv", manv ?? string.Empty);
            DataTable dtBANGLUONGCANHAN = new DataTable();
            da.Fill(dtBANGLUONGCANHAN);
            return dtBANGLUONGCANHAN;
        }

        public string TimMaLuongNVThangNay(string maNV, string thang, string nam)
        {
            string maLuong = string.Empty;
            string ngayDauCuaThang, ngayDauThangsau;
            ngayDauCuaThang = "01/" + thang + "/" + nam;
            if (thang == "12")
            {
                ngayDauThangsau = "01/01/" + (int.Parse(nam + 1)).ToString();
            }
            else ngayDauThangsau = "01/" + (int.Parse(thang + 1)).ToString() + "/" + (int.Parse(nam + 1)).ToString();

            CheckConnection();
            using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 MALUONG FROM THAYDOIBANGLUONG WHERE MANV = @manv AND NGAYSUA < @ngaydau AND NGAYSUA >= @ngaydau2 ORDER BY NGAYSUA DESC", connection))
            {
                cmd.Parameters.AddWithValue("@manv", maNV ?? string.Empty);
                cmd.Parameters.AddWithValue("@ngaydau", ngayDauCuaThang ?? string.Empty);
                cmd.Parameters.AddWithValue("@ngaydau2", ngayDauThangsau ?? string.Empty);
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read()) maLuong = sdr["MALUONG"].ToString();
                }
            }
            connection.Close();
            return maLuong;
        }
    }
}
