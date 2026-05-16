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
    public class DAL_BANGCHAMCONG : KetNoi
    {

        public DataTable getBangChamCong()
        {
            SqlDataAdapter da = new SqlDataAdapter("dbo.usp_BangChamCong_GetAll", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dtBANGCHAMCONG = new DataTable();
            da.Fill(dtBANGCHAMCONG);
            return dtBANGCHAMCONG;
        }

        public DataTable xuatBangChamCong()
        {
            SqlDataAdapter da = new SqlDataAdapter("dbo.usp_BangChamCong_GetAll", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dtBANGCHAMCONG = new DataTable();
            da.Fill(dtBANGCHAMCONG);
            return dtBANGCHAMCONG;
        }

        public bool ThemBangChamCong(DTO_BANGCHAMCONG bangChamCong)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand("dbo.usp_BangChamCong_Insert", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MANV", bangChamCong.Manv);
                    cmd.Parameters.AddWithValue("@THANG", bangChamCong.Thang);
                    cmd.Parameters.AddWithValue("@NAM", bangChamCong.Nam);
                    cmd.Parameters.AddWithValue("@MALUONG", bangChamCong.Maluong ?? string.Empty);
                    cmd.Parameters.AddWithValue("@TIENKHENTHUONG", bangChamCong.Tienkhenthuong);
                    cmd.Parameters.AddWithValue("@TIENKYLUAT", bangChamCong.Tienkyluat);
                    cmd.Parameters.AddWithValue("@SONGAYCONG", bangChamCong.Songaycong);
                    cmd.Parameters.AddWithValue("@SONGAYNGHI", bangChamCong.Songaynghi);
                    cmd.Parameters.AddWithValue("@SOGIOLAMTHEM", bangChamCong.Sogiolamthem);
                    cmd.Parameters.AddWithValue("@GHICHU", bangChamCong.Ghichu ?? string.Empty);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }
        /*
	MANV INT,
	THANG INT,
	NAM INT,
	PRIMARY KEY (MANV, THANG, NAM),
	MALUONG VARCHAR(8),
	MAKT INT,
	MAKL INT,
	SONGAYCONG INT,
	SONGAYNGHI INT,
	SOGIOLAMTHEM INT,
	GHICHU NVARCHAR(60)
 */
        public bool SuaBangChamCong(DTO_BANGCHAMCONG bangChamCong)
        {
            if (connection.State != ConnectionState.Open) connection.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand("dbo.usp_BangChamCong_Update", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MANV", bangChamCong.Manv);
                    cmd.Parameters.AddWithValue("@THANG", bangChamCong.Thang);
                    cmd.Parameters.AddWithValue("@NAM", bangChamCong.Nam);
                    cmd.Parameters.AddWithValue("@MALUONG", bangChamCong.Maluong ?? string.Empty);
                    cmd.Parameters.AddWithValue("@TIENKHENTHUONG", bangChamCong.Tienkhenthuong);
                    cmd.Parameters.AddWithValue("@TIENKYLUAT", bangChamCong.Tienkyluat);
                    cmd.Parameters.AddWithValue("@SONGAYCONG", bangChamCong.Songaycong);
                    cmd.Parameters.AddWithValue("@SONGAYNGHI", bangChamCong.Songaynghi);
                    cmd.Parameters.AddWithValue("@SOGIOLAMTHEM", bangChamCong.Sogiolamthem);
                    cmd.Parameters.AddWithValue("@GHICHU", bangChamCong.Ghichu ?? string.Empty);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }

        public bool XoaBangChamCong(int manv,int thang,int nam)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand("dbo.usp_BangChamCong_Delete", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MANV", manv);
                    cmd.Parameters.AddWithValue("@THANG", thang);
                    cmd.Parameters.AddWithValue("@NAM", nam);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }

        public DTO_BANGCHAMCONG getBangChamCongTheoNhanVien(string maNV, int thang, int nam)
        {
            DTO_BANGCHAMCONG dtoBangChamCong = new DTO_BANGCHAMCONG();
            if (connection.State != ConnectionState.Open) connection.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand("dbo.usp_BangChamCong_GetByManvMonthYear", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MANV", maNV);
                    cmd.Parameters.AddWithValue("@THANG", thang);
                    cmd.Parameters.AddWithValue("@NAM", nam);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            dtoBangChamCong.Songaycong = reader.IsDBNull(0) ? 0 : Convert.ToInt32(reader[0].ToString());
                            dtoBangChamCong.Songaynghi = reader.IsDBNull(1) ? 0 : Convert.ToInt32(reader[1].ToString());
                            dtoBangChamCong.Sogiolamthem = reader.IsDBNull(2) ? 0 : Convert.ToInt32(reader[2].ToString());
                        }
                        return dtoBangChamCong;
                    }
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }

        public DataTable getBangChiTietChamCongTheoNhanVien(string maNV, string thang, string nam)
        {
            DataTable dtBANGCHAMCONG = new DataTable();
            if (thang == "" && nam == "")
            {
                var da = new SqlDataAdapter("dbo.usp_BangChamCong_GetByManvMonthYear", connection);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@MANV", maNV ?? string.Empty);
                da.SelectCommand.Parameters.AddWithValue("@THANG", DBNull.Value);
                da.SelectCommand.Parameters.AddWithValue("@NAM", DBNull.Value);
                da.Fill(dtBANGCHAMCONG);
            }
            else
            {
                var da = new SqlDataAdapter("dbo.usp_BangChamCong_GetByManvMonthYear", connection);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@MANV", maNV ?? string.Empty);
                da.SelectCommand.Parameters.AddWithValue("@THANG", string.IsNullOrEmpty(thang) ? (object)DBNull.Value : (object)thang);
                da.SelectCommand.Parameters.AddWithValue("@NAM", string.IsNullOrEmpty(nam) ? (object)DBNull.Value : (object)nam);
                da.Fill(dtBANGCHAMCONG);
            }
            return dtBANGCHAMCONG;
        }

        public DTO_BANGCHAMCONG getBangChamCongNhanVienTheoThang(string maNV, int thang, int nam)
        {
            DTO_BANGCHAMCONG dtoBangChamCong = new DTO_BANGCHAMCONG();
            if (connection.State != ConnectionState.Open) connection.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT LUONG, BANGCHAMCONG.THANG, BANGCHAMCONG.NAM, BANGCHAMCONG.MALUONG, TIENKHENTHUONG, TIENKYLUAT, SONGAYCONG, SONGAYNGHI, SOGIOLAMTHEM FROM BANGCHAMCONG JOIN BANGTINHLUONG ON BANGCHAMCONG.MANV = BANGTINHLUONG.MANV AND BANGCHAMCONG.THANG = BANGTINHLUONG.THANG AND BANGCHAMCONG.NAM = BANGTINHLUONG.NAM WHERE BANGCHAMCONG.MANV = @manv AND BANGCHAMCONG.THANG = @thang AND BANGCHAMCONG.NAM = @nam", connection))
                {
                    cmd.Parameters.AddWithValue("@manv", maNV ?? string.Empty);
                    cmd.Parameters.AddWithValue("@thang", thang);
                    cmd.Parameters.AddWithValue("@nam", nam);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            dtoBangChamCong.Thang = Convert.ToInt32(reader[1].ToString());
                            dtoBangChamCong.Nam = Convert.ToInt32(reader[2].ToString());
                            dtoBangChamCong.Maluong = reader[3].ToString();
                            dtoBangChamCong.Tienkhenthuong = double.Parse(reader[4].ToString());
                            dtoBangChamCong.Tienkyluat = double.Parse(reader[5].ToString());
                            dtoBangChamCong.Songaycong = int.Parse(reader[6].ToString());
                            dtoBangChamCong.Songaynghi = int.Parse(reader[7].ToString());
                            dtoBangChamCong.Sogiolamthem = int.Parse(reader[8].ToString());
                        }
                        return dtoBangChamCong;
                    }
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }

        public string GetMaLuongTheoThang(string maNV, string thang, string nam)
        {
            string maLuong = string.Empty;
            CheckConnection();
            using (SqlCommand cmd = new SqlCommand("dbo.usp_BangChamCong_GetByManvMonthYear", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MANV", maNV ?? string.Empty);
                cmd.Parameters.AddWithValue("@THANG", string.IsNullOrEmpty(thang) ? (object)DBNull.Value : (object)thang);
                cmd.Parameters.AddWithValue("@NAM", string.IsNullOrEmpty(nam) ? (object)DBNull.Value : (object)nam);
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        if (!sdr.IsDBNull(sdr.GetOrdinal("MALUONG")))
                            maLuong = sdr["MALUONG"].ToString();
                    }
                }
            }
            connection.Close();
            return maLuong;
        }

        public bool KiemTraTonTai(string maNV, string thang, string nam)
        {
            if (connection.State != ConnectionState.Open) connection.Open();
            using (SqlCommand cmd = new SqlCommand("dbo.usp_BangChamCong_GetByManvMonthYear", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MANV", maNV ?? string.Empty);
                cmd.Parameters.AddWithValue("@THANG", string.IsNullOrEmpty(thang) ? (object)DBNull.Value : (object)thang);
                cmd.Parameters.AddWithValue("@NAM", string.IsNullOrEmpty(nam) ? (object)DBNull.Value : (object)nam);
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    return r.Read();
                }
            }
        }

        public bool KiemTraTonTaiNhanVien(string maNV)
        {
            if (connection.State != ConnectionState.Open) connection.Open();
            using (SqlCommand cmd = new SqlCommand("dbo.usp_BangChamCong_GetByManvMonthYear", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MANV", maNV ?? string.Empty);
                cmd.Parameters.AddWithValue("@THANG", DBNull.Value);
                cmd.Parameters.AddWithValue("@NAM", DBNull.Value);
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    return r.Read();
                }
            }
        }

        public bool SuaGhiChu(string ghiChu, string maNV)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            using (SqlCommand cmd = new SqlCommand("dbo.usp_BangChamCong_Update", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                // only update GHICHU field
                cmd.Parameters.AddWithValue("@MANV", maNV ?? string.Empty);
                cmd.Parameters.AddWithValue("@THANG", 0);
                cmd.Parameters.AddWithValue("@NAM", 0);
                cmd.Parameters.AddWithValue("@MALUONG", string.Empty);
                cmd.Parameters.AddWithValue("@TIENKHENTHUONG", 0);
                cmd.Parameters.AddWithValue("@TIENKYLUAT", 0);
                cmd.Parameters.AddWithValue("@SONGAYCONG", 0);
                cmd.Parameters.AddWithValue("@SONGAYNGHI", 0);
                cmd.Parameters.AddWithValue("@SOGIOLAMTHEM", 0);
                cmd.Parameters.AddWithValue("@GHICHU", ghiChu ?? string.Empty);
                return cmd.ExecuteNonQuery() > 0;
            }
            connection.Close();
        }
    }
}
