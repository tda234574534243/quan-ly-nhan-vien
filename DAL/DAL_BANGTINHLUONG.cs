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
    public class DAL_BANGTINHLUONG : KetNoi
    {
        /*		MANV INT,
	LUONG MONEY,
	THANG INT,
	NAM INT,
	PRIMARY KEY (MANV, THANG, NAM),
	GHICHU NVARCHAR(80)*/
        public DataTable getBangTinhLuong()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT MANV 'Mã nhân viên', LUONG 'Lương', THANG 'Tháng', NAM 'Năm', GHICHU 'Ghi chú' FROM BANGTINHLUONG", connection);
            DataTable dtBANGTINHLUONG = new DataTable();
            da.Fill(dtBANGTINHLUONG);
            return dtBANGTINHLUONG;
        }
        public bool ThemBangTinhLuong(DTO_BANGTINHLUONG bangTinhLuong)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                string sql = "INSERT INTO BANGTINHLUONG(MANV, LUONG, THANG, NAM, GHICHU) VALUES(@manv,@luong,@thang,@nam,@ghichu)";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@manv", bangTinhLuong.Manv);
                    cmd.Parameters.AddWithValue("@luong", bangTinhLuong.Luong);
                    cmd.Parameters.AddWithValue("@thang", bangTinhLuong.Thang);
                    cmd.Parameters.AddWithValue("@nam", bangTinhLuong.Nam);
                    cmd.Parameters.AddWithValue("@ghichu", bangTinhLuong.Ghichu ?? string.Empty);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }
        public bool SuaBangTinhLuong(DTO_BANGTINHLUONG bangTinhLuong)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                string sql = "UPDATE BANGTINHLUONG SET LUONG=@luong, GHICHU=@ghichu WHERE MANV=@manv AND THANG=@thang AND NAM=@nam";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@luong", bangTinhLuong.Luong);
                    cmd.Parameters.AddWithValue("@ghichu", bangTinhLuong.Ghichu ?? string.Empty);
                    cmd.Parameters.AddWithValue("@manv", bangTinhLuong.Manv);
                    cmd.Parameters.AddWithValue("@thang", bangTinhLuong.Thang);
                    cmd.Parameters.AddWithValue("@nam", bangTinhLuong.Nam);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }

        public bool XoaBangTinhLuong(int manv, int thang, int nam)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand("DELETE FROM BANGTINHLUONG WHERE MANV=@manv AND THANG=@thang AND NAM=@nam", connection))
                {
                    cmd.Parameters.AddWithValue("@manv", manv);
                    cmd.Parameters.AddWithValue("@thang", thang);
                    cmd.Parameters.AddWithValue("@nam", nam);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }

        public DataTable getBangTinhLuongTheoThang(string thang, string nam)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            if (thang == "")
            {
                da = new SqlDataAdapter("SELECT MANV 'Mã nhân viên', LUONG 'Lương', THANG 'Tháng', NAM 'Năm', GHICHU 'Ghi chú' FROM BANGTINHLUONG WHERE NAM = @nam", connection);
                da.SelectCommand.Parameters.AddWithValue("@nam", nam ?? string.Empty);
            }
            else
            {
                da = new SqlDataAdapter("SELECT MANV 'Mã nhân viên', LUONG 'Lương', THANG 'Tháng', NAM 'Năm', GHICHU 'Ghi chú' FROM BANGTINHLUONG WHERE THANG = @thang AND NAM = @nam", connection);
                da.SelectCommand.Parameters.AddWithValue("@thang", thang ?? string.Empty);
                da.SelectCommand.Parameters.AddWithValue("@nam", nam ?? string.Empty);
            }
            DataTable dtBANGTINHLUONG = new DataTable();
            da.Fill(dtBANGTINHLUONG);
            return dtBANGTINHLUONG;
        }

        public bool KiemTraTonTaiNhanVien(string maNV)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            string sql = string.Format("SELECT * FROM BANGTINHLUONG WHERE MANV='{0}'", maNV);
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

        public bool SuaGhiChu(string ghiChu, string maNV)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            string sql = string.Format("UPDATE BANGTINHLUONG " +
                "SET GHICHU=N'{0}' WHERE MANV = '{1}'", ghiChu, maNV);
            SqlCommand cmd = new SqlCommand(sql, connection);
            if (cmd.ExecuteNonQuery() > 0)
                return true;
            else return false;
            connection.Close();
        }
    }
}
