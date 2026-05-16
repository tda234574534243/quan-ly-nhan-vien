using DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace DAL
{
    public class DAL_NHANVIEN : KetNoi
    {

        public DataTable getNhanVien()
        {
            SqlDataAdapter da = new SqlDataAdapter("dbo.usp_NhanVien_GetAll", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dtNHANVIEN = new DataTable();
            da.Fill(dtNHANVIEN);
            return dtNHANVIEN;
        }

        public DataTable xuatNhanVien()
        {
            SqlDataAdapter da = new SqlDataAdapter("dbo.usp_NhanVien_GetAll", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dtNHANVIEN = new DataTable();
            da.Fill(dtNHANVIEN);
            return dtNHANVIEN;
        }

        public bool ThemNhanVien(DTO_NHANVIEN nhanVien)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand("dbo.usp_NhanVien_Insert", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MAPHONG", nhanVien.Maphong ?? string.Empty);
                    cmd.Parameters.AddWithValue("@MALUONG", nhanVien.Maluong ?? string.Empty);
                    cmd.Parameters.AddWithValue("@HOTEN", nhanVien.Hoten ?? string.Empty);
                    cmd.Parameters.AddWithValue("@NGAYSINH", nhanVien.Ngaysinh == default(DateTime) ? (object)DBNull.Value : (object)nhanVien.Ngaysinh);
                    cmd.Parameters.AddWithValue("@GIOITINH", nhanVien.Gioitinh ?? string.Empty);
                    cmd.Parameters.AddWithValue("@DANTOC", nhanVien.Dantoc ?? string.Empty);
                    cmd.Parameters.AddWithValue("@CMND_CCCD", nhanVien.Cmnd_cccd ?? string.Empty);
                    cmd.Parameters.AddWithValue("@NOICAP", nhanVien.Noicap ?? string.Empty);
                    cmd.Parameters.AddWithValue("@CHUCVU", nhanVien.Chucvu ?? string.Empty);
                    cmd.Parameters.AddWithValue("@MALOAINV", nhanVien.Maloainv ?? string.Empty);
                    cmd.Parameters.AddWithValue("@LOAIHD", nhanVien.Loaihd ?? string.Empty);
                    cmd.Parameters.AddWithValue("@THOIGIAN", nhanVien.Thoigian);
                    cmd.Parameters.AddWithValue("@NGAYKY", nhanVien.Ngaydangki == default(DateTime) ? (object)DBNull.Value : (object)nhanVien.Ngaydangki);
                    cmd.Parameters.AddWithValue("@NGAYHETHAN", nhanVien.Ngayhethan == default(DateTime) ? (object)DBNull.Value : (object)nhanVien.Ngayhethan);
                    cmd.Parameters.AddWithValue("@SDT", nhanVien.Sdt ?? string.Empty);
                    cmd.Parameters.AddWithValue("@HOCVAN", nhanVien.Hocvan ?? string.Empty);
                    cmd.Parameters.AddWithValue("@GHICHU", nhanVien.Ghichu ?? string.Empty);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }
        /*
    MANV INT IDENTITY(1,1) PRIMARY KEY,
	MAPHONG VARCHAR(6),
	MALUONG VARCHAR(8),
	HOTEN NVARCHAR(70),
	NGAYSINH DATETIME,
	GIOITINH NVARCHAR(3),
	DANTOC NVARCHAR(12),
	CMND_CCCD VARCHAR(12),
	NOICAP NVARCHAR(20),
	CHUCVU NVARCHAR(25),
	MALOAINV VARCHAR(10),
	LOAIHD NVARCHAR(20),
	THOIGIAN INT,
	NGAYKY DATETIME,
	NGAYHETHAN DATETIME,
	SDT VARCHAR(10),
	HOCVAN NVARCHAR(20),
	GHICHU NVARCHAR(60)
 */
        public bool SuaNhanVien(DTO_NHANVIEN nhanVien)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand("dbo.usp_NhanVien_Update", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MANV", nhanVien.Manv);
                    cmd.Parameters.AddWithValue("@MAPHONG", nhanVien.Maphong ?? string.Empty);
                    cmd.Parameters.AddWithValue("@MALUONG", nhanVien.Maluong ?? string.Empty);
                    cmd.Parameters.AddWithValue("@HOTEN", nhanVien.Hoten ?? string.Empty);
                    cmd.Parameters.AddWithValue("@NGAYSINH", nhanVien.Ngaysinh == default(DateTime) ? (object)DBNull.Value : (object)nhanVien.Ngaysinh);
                    cmd.Parameters.AddWithValue("@GIOITINH", nhanVien.Gioitinh ?? string.Empty);
                    cmd.Parameters.AddWithValue("@DANTOC", nhanVien.Dantoc ?? string.Empty);
                    cmd.Parameters.AddWithValue("@CMND_CCCD", nhanVien.Cmnd_cccd ?? string.Empty);
                    cmd.Parameters.AddWithValue("@NOICAP", nhanVien.Noicap ?? string.Empty);
                    cmd.Parameters.AddWithValue("@CHUCVU", nhanVien.Chucvu ?? string.Empty);
                    cmd.Parameters.AddWithValue("@MALOAINV", nhanVien.Maloainv ?? string.Empty);
                    cmd.Parameters.AddWithValue("@LOAIHD", nhanVien.Loaihd ?? string.Empty);
                    cmd.Parameters.AddWithValue("@THOIGIAN", nhanVien.Thoigian);
                    cmd.Parameters.AddWithValue("@NGAYKY", nhanVien.Ngaydangki == default(DateTime) ? (object)DBNull.Value : (object)nhanVien.Ngaydangki);
                    cmd.Parameters.AddWithValue("@NGAYHETHAN", nhanVien.Ngayhethan == default(DateTime) ? (object)DBNull.Value : (object)nhanVien.Ngayhethan);
                    cmd.Parameters.AddWithValue("@SDT", nhanVien.Sdt ?? string.Empty);
                    cmd.Parameters.AddWithValue("@HOCVAN", nhanVien.Hocvan ?? string.Empty);
                    cmd.Parameters.AddWithValue("@GHICHU", nhanVien.Ghichu ?? string.Empty);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }

        public bool XoaNhanVien(int manv)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand("dbo.usp_NhanVien_Delete", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MANV", manv);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }

        public DataTable TongHopNhanVienTheoPhong(string maPhong, string ten)
        {
            DataTable dtNHANVIEN = new DataTable();

            // Use stored procedure search to fetch appropriate rows
            SqlDataAdapter da = new SqlDataAdapter("dbo.usp_NhanVien_Search", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.AddWithValue("@MAPHONG", string.IsNullOrEmpty(maPhong) ? (object)DBNull.Value : (object)maPhong);
            da.SelectCommand.Parameters.AddWithValue("@TEN", string.IsNullOrEmpty(ten) ? (object)DBNull.Value : (object) ("%" + ten + "%"));
            da.Fill(dtNHANVIEN);
            return dtNHANVIEN;
        }

        public List<string> TongHopMaNhanVienTheoGioiTinh(string gioiTinh)
        {
            List<string> listMaNhanVien = new List<string>();
            CheckConnection();
            // retrieve via stored procedure and filter client-side
            using (SqlCommand cmd = new SqlCommand("dbo.usp_NhanVien_GetAll", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        if (!sdr.IsDBNull(5) && sdr[5].ToString() == (gioiTinh ?? string.Empty))
                            listMaNhanVien.Add(sdr[0].ToString());
                    }
                }
            }
            connection.Close();
            return listMaNhanVien;
        }

        public List<string> TongHopMaNhanVien()
        {
            List<string> listMaNhanVien = new List<string>();
            CheckConnection();
            using (SqlCommand cmd = new SqlCommand("dbo.usp_NhanVien_GetAll", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read()) listMaNhanVien.Add(sdr[0].ToString());
                }
            }
            connection.Close();
            return listMaNhanVien;
        }

        public string TimTenNVTheoMa(string maNV)
        {
            string tenNV = string.Empty;
            CheckConnection();
            using (SqlCommand cmd = new SqlCommand("dbo.usp_NhanVien_GetNameById", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MANV", int.TryParse(maNV, out int id) ? id : 0);
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    if (sdr.Read()) tenNV = sdr[0].ToString();
                }
            }
            connection.Close();
            return tenNV;
        }

        public int TimNamDauTienNVVaoLam()
        {
            int nam = 1990;
            CheckConnection();
            using (SqlCommand cmd = new SqlCommand("dbo.usp_NhanVien_GetAll", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    DateTime? min = null;
                    while (sdr.Read())
                    {
                        if (!sdr.IsDBNull(13))
                        {
                            var d = DateTime.Parse(sdr[13].ToString());
                            if (!min.HasValue || d < min.Value) min = d;
                        }
                    }
                    if (min.HasValue) nam = min.Value.Year;
                }
            }
            connection.Close();
            return nam;
        }

        public int TimNamGanNhatNVVaoLam()
        {
            int nam = 1990;
            CheckConnection();
            using (SqlCommand cmd = new SqlCommand("dbo.usp_NhanVien_GetAll", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    DateTime? max = null;
                    while (sdr.Read())
                    {
                        if (!sdr.IsDBNull(13))
                        {
                            var d = DateTime.Parse(sdr[13].ToString());
                            if (!max.HasValue || d > max.Value) max = d;
                        }
                    }
                    if (max.HasValue) nam = max.Value.Year;
                }
            }
            connection.Close();
            return nam;
        }

        public int TimMaNVTheoTen(string tenNV)
        {
            int maNV = 0;
            CheckConnection();
            using (SqlCommand cmd = new SqlCommand("dbo.usp_NhanVien_GetAll", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        if (!sdr.IsDBNull(3) && sdr[3].ToString() == (tenNV ?? string.Empty))
                        {
                            maNV = int.Parse(sdr[0].ToString());
                            break;
                        }
                    }
                }
            }
            connection.Close();
            return maNV;
        }



        public bool SuaMaLuongNhanVien(string maNV, string maLuong)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                // Load existing record to preserve other fields, then call stored procedure update
                DataTable all = getNhanVien();
                DataRow[] rows = all.Select("MANV = " + maNV);
                if (rows.Length == 0) return false;
                var r = rows[0];
                using (SqlCommand cmd = new SqlCommand("dbo.usp_NhanVien_Update", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MANV", int.TryParse(maNV, out int id) ? id : 0);
                    cmd.Parameters.AddWithValue("@MAPHONG", r[1] ?? string.Empty);
                    cmd.Parameters.AddWithValue("@MALUONG", maLuong ?? string.Empty);
                    cmd.Parameters.AddWithValue("@HOTEN", r[3] ?? string.Empty);
                    cmd.Parameters.AddWithValue("@NGAYSINH", r.IsNull(4) ? (object)DBNull.Value : r[4]);
                    cmd.Parameters.AddWithValue("@GIOITINH", r[5] ?? string.Empty);
                    cmd.Parameters.AddWithValue("@DANTOC", r[6] ?? string.Empty);
                    cmd.Parameters.AddWithValue("@CMND_CCCD", r[7] ?? string.Empty);
                    cmd.Parameters.AddWithValue("@NOICAP", r[8] ?? string.Empty);
                    cmd.Parameters.AddWithValue("@CHUCVU", r[9] ?? string.Empty);
                    cmd.Parameters.AddWithValue("@MALOAINV", r[10] ?? string.Empty);
                    cmd.Parameters.AddWithValue("@LOAIHD", r[11] ?? string.Empty);
                    cmd.Parameters.AddWithValue("@THOIGIAN", r.IsNull(12) ? 0 : Convert.ToInt32(r[12]));
                    cmd.Parameters.AddWithValue("@NGAYKY", r.IsNull(13) ? (object)DBNull.Value : r[13]);
                    cmd.Parameters.AddWithValue("@NGAYHETHAN", r.IsNull(14) ? (object)DBNull.Value : r[14]);
                    cmd.Parameters.AddWithValue("@SDT", r[15] ?? string.Empty);
                    cmd.Parameters.AddWithValue("@HOCVAN", r[16] ?? string.Empty);
                    cmd.Parameters.AddWithValue("@GHICHU", r[17] ?? string.Empty);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }

        public int SoLuongNhanVienVaoLam (int thang,int nam)
        {
            int n = 0;
            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand("dbo.usp_NhanVien_GetAll", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            if (!sdr.IsDBNull(13))
                            {
                                var d = DateTime.Parse(sdr[13].ToString());
                                if (d.Month == thang && d.Year == nam) n++;
                            }
                        }
                    }
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
            connection.Close();
            return n;
        }

        public DTO_NHANVIEN GetChiTietNhanVienTheoMa(string maNV)
        {
            DTO_NHANVIEN dtoNhanVien = new DTO_NHANVIEN();
            if (connection.State != ConnectionState.Open)
                connection.Open();
            using (SqlCommand cmd = new SqlCommand("dbo.usp_NhanVien_GetAll", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader[0].ToString() == maNV)
                        {
                            dtoNhanVien.Manv = int.Parse(maNV);
                            dtoNhanVien.Maphong = reader[1].ToString();
                            dtoNhanVien.Maluong = reader[2].ToString();
                            dtoNhanVien.Hoten = reader[3].ToString();
                            dtoNhanVien.Ngaysinh = reader.IsDBNull(4) ? default(DateTime) : DateTime.Parse(reader[4].ToString());
                            dtoNhanVien.Gioitinh = reader[5].ToString();
                            dtoNhanVien.Dantoc = reader[6].ToString();
                            dtoNhanVien.Cmnd_cccd = reader[7].ToString();
                            dtoNhanVien.Noicap = reader[8].ToString();
                            dtoNhanVien.Chucvu = reader[9].ToString();
                            dtoNhanVien.Maloainv = reader[10].ToString();
                            dtoNhanVien.Loaihd = reader[11].ToString();
                            dtoNhanVien.Thoigian = reader.IsDBNull(12) ? 0 : int.Parse(reader[12].ToString());
                            dtoNhanVien.Ngaydangki = reader.IsDBNull(13) ? default(DateTime) : DateTime.Parse(reader[13].ToString());
                            dtoNhanVien.Ngayhethan = reader.IsDBNull(14) ? default(DateTime) : DateTime.Parse(reader[14].ToString());
                            dtoNhanVien.Sdt = reader[15].ToString();
                            dtoNhanVien.Hocvan = reader[16].ToString();
                            dtoNhanVien.Ghichu = reader[17].ToString();
                            return dtoNhanVien;
                        }
                    }
                }
            }
            return dtoNhanVien;
        }

        public DataTable TimKiemNVTheoMa(string manv)
        {
            SqlDataAdapter da = new SqlDataAdapter("dbo.usp_NhanVien_Search", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.AddWithValue("@MAPHONG", string.Empty);
            da.SelectCommand.Parameters.AddWithValue("@TEN", string.Empty);
            DataTable dtNHANVIEN = new DataTable();
            da.Fill(dtNHANVIEN);
            // filter client-side since stored proc doesn't support partial MANV search
            DataRow[] rows = dtNHANVIEN.Select("CONVERT(MANV, 'System.String') LIKE '%" + manv + "%'");
            DataTable dt = dtNHANVIEN.Clone();
            foreach (var r in rows) dt.ImportRow(r);
            return dt;
        }
        public DataTable TimKiemNVTheoTen(string ten)
        {
            SqlDataAdapter da = new SqlDataAdapter("dbo.usp_NhanVien_Search", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.AddWithValue("@MAPHONG", string.Empty);
            da.SelectCommand.Parameters.AddWithValue("@TEN", "%" + (ten ?? string.Empty) + "%");
            DataTable dtNHANVIEN = new DataTable();
            da.Fill(dtNHANVIEN);
            return dtNHANVIEN;
        }
        public DataTable TimKiemNVTheoSDT(string sdt)
        {
            SqlDataAdapter da = new SqlDataAdapter("dbo.usp_NhanVien_GetAll", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dtNHANVIEN = new DataTable();
            da.Fill(dtNHANVIEN);
            DataRow[] rows = dtNHANVIEN.Select("SDT LIKE '%" + (sdt ?? string.Empty) + "%'");
            DataTable dt = dtNHANVIEN.Clone();
            foreach (var r in rows) dt.ImportRow(r);
            return dt;
        }

        public string GetMaLuong(string maNV)
        {
            string maLuong = string.Empty;
            CheckConnection();
            using (SqlCommand cmd = new SqlCommand("dbo.usp_NhanVien_GetNameById", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MANV", int.TryParse(maNV, out int id) ? id : 0);
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    if (sdr.Read())
                    {
                        maLuong = sdr[0].ToString();
                    }
                }
            }
            connection.Close();
            return maLuong;
        }
    }
}
