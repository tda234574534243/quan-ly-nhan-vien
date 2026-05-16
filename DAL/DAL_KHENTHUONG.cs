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
    public class DAL_KHENTHUONG : KetNoi
    {

        public DataTable getKhenThuong()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT MAKT 'Mã khen thưởng', TIEN 'Tiền', LYDO 'Lý do' FROM KHENTHUONG", connection);
            DataTable dtKHENTHUONG = new DataTable();
            da.Fill(dtKHENTHUONG);
            return dtKHENTHUONG;
        }
        public bool ThemKhenThuong(DTO_KHENTHUONG khenThuong)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                string sql = "INSERT INTO KHENTHUONG(TIEN, LYDO) VALUES(@tien, @lydo)";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@tien", khenThuong.Tien);
                    cmd.Parameters.AddWithValue("@lydo", khenThuong.Lydo ?? string.Empty);
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }
        /*
	MAKL INT PRIMARY KEY,
	TIEN MONEY,
	LYDO NVARCHAR(50)
 */
        public bool SuaKhenThuong(DTO_KHENTHUONG khenThuong)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                string sql = "UPDATE KHENTHUONG SET TIEN=@tien, LYDO=@lydo WHERE MAKT=@makt";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@tien", khenThuong.Tien);
                    cmd.Parameters.AddWithValue("@lydo", khenThuong.Lydo ?? string.Empty);
                    cmd.Parameters.AddWithValue("@makt", khenThuong.Makt);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }

        public bool XoaKhenThuong(int makt)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand("DELETE FROM KHENTHUONG WHERE MAKT = @makt", connection))
                {
                    cmd.Parameters.AddWithValue("@makt", makt);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }

        public List<string> TongHopMaKhenThuong()
        {
            List<string> listMaKhenThuong = new List<string>();
            CheckConnection();
            using (SqlCommand cmd = new SqlCommand("SELECT MAKT FROM KHENTHUONG", connection))
            using (SqlDataReader sdr = cmd.ExecuteReader())
            {
                while (sdr.Read()) listMaKhenThuong.Add(sdr[0].ToString());
            }
            connection.Close();
            return listMaKhenThuong;
        }
    }
}
