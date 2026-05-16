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
    public class DAL_LOAINHANVIEN : KetNoi
    {

        public DataTable getLoaiNhanVien()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM LOAINHANVIEN", connection);
            DataTable dtLOAINHANVIEN = new DataTable();
            da.Fill(dtLOAINHANVIEN);
            return dtLOAINHANVIEN;
        }
        public bool ThemLoaiNhanVien(DTO_LOAINHANVIEN loaiNhanVien)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                string sql = "INSERT INTO LOAINHANVIEN(MALOAINV, TENLOAINV, MUCLUONGCOBAN) VALUES(@maloainv, @tenloai, @mucluong)";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@maloainv", loaiNhanVien.Maloainv ?? string.Empty);
                    cmd.Parameters.AddWithValue("@tenloai", loaiNhanVien.Tenloainv ?? string.Empty);
                    cmd.Parameters.AddWithValue("@mucluong", loaiNhanVien.Mucluongcoban);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }
        /*
	MALOAINV VARCHAR(10) PRIMARY KEY,
	TENLOAINV NVARCHAR(30),
	MUCLUONGCOBAN MONEY,
 */
        public bool SuaLoaiNhanVien(DTO_LOAINHANVIEN loaiNhanVien)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                string sql = "UPDATE LOAINHANVIEN SET TENLOAINV=@ten, MUCLUONGCOBAN=@mucluong WHERE MALOAINV = @maloainv";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@ten", loaiNhanVien.Tenloainv ?? string.Empty);
                    cmd.Parameters.AddWithValue("@mucluong", loaiNhanVien.Mucluongcoban);
                    cmd.Parameters.AddWithValue("@maloainv", loaiNhanVien.Maloainv ?? string.Empty);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }

        public bool XoaLoaiNhanVien(string maloainv)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                string sql = "DELETE FROM LOAINHANVIEN WHERE MALOAINV = @maloainv";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@maloainv", maloainv ?? string.Empty);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }

        public string TimKiemTheoLoaiNhanVien(string loaiNV)
        {
            string maLoaiNV = string.Empty;
            CheckConnection();
            using (SqlCommand cmd = new SqlCommand("SELECT MALOAINV FROM LOAINHANVIEN WHERE TENLOAINV = @ten", connection))
            {
                cmd.Parameters.AddWithValue("@ten", loaiNV ?? string.Empty);
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read()) maLoaiNV = sdr["MALOAINV"].ToString();
                }
            }
            connection.Close();
            return maLoaiNV;
        }

        public string TimKiemTheoMaLoaiNhanVien(string maLoaiNV)
        {
            string loaiNV = string.Empty;
            CheckConnection();
            using (SqlCommand cmd = new SqlCommand("SELECT TENLOAINV FROM LOAINHANVIEN WHERE MALOAINV = @maloainv", connection))
            {
                cmd.Parameters.AddWithValue("@maloainv", maLoaiNV ?? string.Empty);
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read()) loaiNV = sdr["TENLOAINV"].ToString();
                }
            }
            connection.Close();
            return loaiNV;
        }

        public List<string> TongHopLoaiNhanVien()
        {
            List<string> listLoaiNhanVien = new List<string>();
            CheckConnection();
            using (SqlCommand cmd = new SqlCommand("SELECT TENLOAINV FROM LOAINHANVIEN", connection))
            using (SqlDataReader sdr = cmd.ExecuteReader())
            {
                while (sdr.Read()) listLoaiNhanVien.Add(sdr[0].ToString());
            }
            connection.Close();
            return listLoaiNhanVien;
        }
    }
}
