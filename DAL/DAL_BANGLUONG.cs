using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DAL_BANGLUONG : KetNoi
    {

        public DataTable getBangLuong()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT MALUONG 'Mã lương', LCB 'Lương cơ bản', PHUCAPCHUCVU 'Phụ cấp chức vụ', PHUCAPKHAC 'Phụ cấp khác', GHICHU 'Ghi chú' FROM BANGLUONG", connection);
            DataTable dtBANGLUONG = new DataTable();
            da.Fill(dtBANGLUONG);
            return dtBANGLUONG;
        }
        public bool ThemBangLuong(DTO_BANGLUONG bangLuong)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                string sql = "INSERT INTO BANGLUONG(MALUONG, LCB, PHUCAPCHUCVU, PHUCAPKHAC, GHICHU) VALUES(@maluong,@lcb,@pcvc,@pck,@ghichu)";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@maluong", bangLuong.Maluong ?? string.Empty);
                    cmd.Parameters.AddWithValue("@lcb", bangLuong.Lcb);
                    cmd.Parameters.AddWithValue("@pcvc", bangLuong.Phucapchucvu);
                    cmd.Parameters.AddWithValue("@pck", bangLuong.Phucapkhac);
                    cmd.Parameters.AddWithValue("@ghichu", bangLuong.Ghichu ?? string.Empty);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }
        /*
    MALUONG VARCHAR(8) PRIMARY KEY,
	LCB MONEY,
	PHUCAPCHUCVU MONEY,
	PHUCAPKHAC MONEY,
	GHICHU NVARCHAR(80)
 */
        public bool SuaBangLuong(DTO_BANGLUONG bangLuong)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                string sql = "UPDATE BANGLUONG SET LCB=@lcb, PHUCAPCHUCVU=@pcvc, PHUCAPKHAC=@pck, GHICHU=@ghichu WHERE MALUONG=@maluong";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@lcb", bangLuong.Lcb);
                    cmd.Parameters.AddWithValue("@pcvc", bangLuong.Phucapchucvu);
                    cmd.Parameters.AddWithValue("@pck", bangLuong.Phucapkhac);
                    cmd.Parameters.AddWithValue("@ghichu", bangLuong.Ghichu ?? string.Empty);
                    cmd.Parameters.AddWithValue("@maluong", bangLuong.Maluong ?? string.Empty);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }

        public bool XoaBangLuong(string maluong)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand("DELETE FROM BANGLUONG WHERE MALUONG = @maluong", connection))
                {
                    cmd.Parameters.AddWithValue("@maluong", maluong ?? string.Empty);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }

        public List<string> TongHopMaLuong()
        {
            List<string> listMaLuong = new List<string>();
            CheckConnection();
            using (SqlCommand cmd = new SqlCommand("SELECT MALUONG FROM BANGLUONG", connection))
            using (SqlDataReader sdr = cmd.ExecuteReader())
            {
                while (sdr.Read()) listMaLuong.Add(sdr[0].ToString());
            }
            connection.Close();
            return listMaLuong;
        }

        public DTO_BANGLUONG GetChiTietLuong(string maluong)
        {
            DTO_BANGLUONG dtoBangLuong = new DTO_BANGLUONG();
            if (connection.State != ConnectionState.Open) connection.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT MALUONG, LCB, PHUCAPCHUCVU, PHUCAPKHAC, GHICHU FROM BANGLUONG WHERE MALUONG=@maluong", connection))
                {
                    cmd.Parameters.AddWithValue("@maluong", maluong ?? string.Empty);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            dtoBangLuong.Maluong = !reader.IsDBNull(0) ? reader.GetString(0) : (maluong ?? string.Empty);
                            dtoBangLuong.Lcb = !reader.IsDBNull(1) ? Convert.ToDouble(reader[1].ToString()) : 0.0;
                            dtoBangLuong.Phucapchucvu = !reader.IsDBNull(2) ? Convert.ToDouble(reader[2].ToString()) : 0.0;
                            dtoBangLuong.Phucapkhac = !reader.IsDBNull(3) ? Convert.ToDouble(reader[3].ToString()) : 0.0;
                            dtoBangLuong.Ghichu = (reader.FieldCount > 4 && !reader.IsDBNull(4)) ? reader.GetString(4) : string.Empty;
                        }
                        return dtoBangLuong;
                    }
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }
    }
}
