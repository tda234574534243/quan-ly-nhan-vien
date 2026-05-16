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
    public class DAL_BAOCAOLUONG : KetNoi
    {
        /*	THANG INT,
	NAM INT,
	TONGTIEN MONEY,
	PRIMARY KEY (THANG, NAM),
	GHICHU NVARCHAR(50)*/
        public DataTable getBaoCaoLuong()
        {
            SqlCommand cmd = new SqlCommand("dbo.usp_BaoCaoLuong_GetAll", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dtBAOCAOLUONG = new DataTable();
            da.Fill(dtBAOCAOLUONG);
            return dtBAOCAOLUONG;
        }
        public bool ThemBaoCaoLuong(DTO_BAOCAOLUONG baoCaoLuong)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand("dbo.usp_BaoCaoLuong_Insert", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@THANG", baoCaoLuong.Thang);
                    cmd.Parameters.AddWithValue("@NAM", baoCaoLuong.Nam);
                    cmd.Parameters.AddWithValue("@GHICHU", baoCaoLuong.Ghichu ?? string.Empty);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }
        public bool SuaBaoCaoLuong(DTO_BAOCAOLUONG baoCaoLuong)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand("dbo.usp_BaoCaoLuong_Update", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@THANG", baoCaoLuong.Thang);
                    cmd.Parameters.AddWithValue("@NAM", baoCaoLuong.Nam);
                    cmd.Parameters.AddWithValue("@GHICHU", baoCaoLuong.Ghichu ?? string.Empty);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }

        public bool XoaBaoCaoLuong(int thang, int nam)
        {
            try
            {
                if (connection.State != ConnectionState.Open) connection.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_BaoCaoLuong_Delete", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@THANG", thang);
                    cmd.Parameters.AddWithValue("@NAM", nam);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }
    }
}
