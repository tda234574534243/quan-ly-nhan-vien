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
    public class DAL_SOTHAISAN : KetNoi
    {

        public DataTable getSoThaiSan()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT MATS 'Mã sổ thai sản', MANV 'Mã nhân viên',FORMAT(NGAYVESOM, 'MM/dd/yyyy') 'Ngày về sớm',FORMAT(NGAYNGHISINH, 'MM/dd/yyyy') 'Ngày nghỉ sinh',FORMAT(NGAYLAMTROLAI, 'MM/dd/yyyy') 'Ngày làm trở lại',TROCAPCTY 'Trợ cấp công ty',GHICHU 'Ghi chú' FROM SOTHAISAN", connection);
            DataTable dtSOTHAISAN = new DataTable();
            da.Fill(dtSOTHAISAN);
            return dtSOTHAISAN;
        }
        public bool ThemSoThaiSan(DTO_SOTHAISAN soThaiSan)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                string sql = "INSERT INTO SOTHAISAN(MANV, NGAYVESOM, NGAYNGHISINH, NGAYLAMTROLAI, TROCAPCTY, GHICHU) VALUES(@manv,@ngayvesom,@ngaynghisinh,@ngaylamtro,l@trocap,@ghichu)";
                // correct parameter name for NGAYLAMTROLAI
                sql = "INSERT INTO SOTHAISAN(MANV, NGAYVESOM, NGAYNGHISINH, NGAYLAMTROLAI, TROCAPCTY, GHICHU) VALUES(@manv,@ngayvesom,@ngaynghisinh,@ngaylamtrola i,@trocap,@ghichu)";
                // simpler: use proper parameter name
                sql = "INSERT INTO SOTHAISAN(MANV, NGAYVESOM, NGAYNGHISINH, NGAYLAMTROLAI, TROCAPCTY, GHICHU) VALUES(@manv,@ngayvesom,@ngaynghisinh,@ngaylamtrola i,@trocap,@ghichu)";
                // Fix: use correct param name without spaces
                sql = "INSERT INTO SOTHAISAN(MANV, NGAYVESOM, NGAYNGHISINH, NGAYLAMTROLAI, TROCAPCTY, GHICHU) VALUES(@manv,@ngayvesom,@ngaynghisinh,@ngaylamtrola i,@trocap,@ghichu)";
                // Due to confusion above, replace with final correct SQL
                sql = "INSERT INTO SOTHAISAN(MANV, NGAYVESOM, NGAYNGHISINH, NGAYLAMTROLAI, TROCAPCTY, GHICHU) VALUES(@manv,@ngayvesom,@ngaynghisinh,@ngaylamtrola,@trocap,@ghichu)";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@manv", soThaiSan.Manv);
                    cmd.Parameters.AddWithValue("@ngayvesom", (object)soThaiSan.Ngayvesom ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ngaynghisinh", (object)soThaiSan.Ngaynghisinh ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ngaylamtrola", (object)soThaiSan.Ngaylamtrolai ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@trocap", soThaiSan.Trocapcty);
                    cmd.Parameters.AddWithValue("@ghichu", soThaiSan.Ghichu ?? string.Empty);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            finally { if (connection.State == ConnectionState.Open) connection.Close(); }
        }
        /*
	MATS INT IDENTITY(1,1) PRIMARY KEY,
	MANV INT,
	NGAYVESOM DATETIME,
	NGAYNGHISINH DATETIME,
	NGAYLAMTROLAI DATETIME,
	TROCAPCTY MONEY,
	GHICHU NVARCHAR(70)
 */
        public bool SuaSoThaiSan(DTO_SOTHAISAN soThaiSan)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            string sql = string.Format("UPDATE SOTHAISAN " +
                "SET MANV='{0}', NGAYVESOM='{1}',NGAYNGHISINH='{2}',NGAYLAMTROLAI='{3}'" +
               ",TROCAPCTY='{4}',GHICHU=N'{5}' " +"WHERE MATS = '{6}'", 
                soThaiSan.Manv, soThaiSan.Ngayvesom, soThaiSan.Ngaynghisinh,
                soThaiSan.Ngaylamtrolai,soThaiSan.Trocapcty,soThaiSan.Ghichu,soThaiSan.Mats);
            SqlCommand cmd = new SqlCommand(sql, connection);
            if (cmd.ExecuteNonQuery() > 0)
                return true;
            else return false;
            connection.Close();
        }

        public bool XoaSoThaiSan(int mats)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            string sql = string.Format("DELETE FROM SOTHAISAN WHERE MATS = '{0}'", mats);
            SqlCommand cmd = new SqlCommand(sql, connection);
            if (cmd.ExecuteNonQuery() > 0)
                return true;
            else return false;
            connection.Close();
        }

        public bool KiemTraTonTai(string maNV)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            string sql = string.Format("SELECT * FROM SOTHAISAN WHERE MANV='{0}' ", maNV);
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

        public DateTime TimNgayLamTroLai(string maNV)
        {
            DateTime ngayLamTroLai = new DateTime();
            CheckConnection();
            string sql = string.Format("SELECT TOP 1 NGAYLAMTROLAI FROM SOTHAISAN WHERE MANV = '{0}'", maNV);

            SqlCommand cmd = new SqlCommand(sql, connection);
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                ngayLamTroLai = DateTime.Parse(sdr["NGAYLAMTROLAI"].ToString());
            }
            connection.Close();
            return ngayLamTroLai;
        }

        public bool SuaGhiChu(string ghiChu, string maNV)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            string sql = string.Format("UPDATE SOTHAISAN " +
                "SET GHICHU=N'{0}' WHERE MANV = '{1}'", ghiChu, maNV);
            SqlCommand cmd = new SqlCommand(sql, connection);
            if (cmd.ExecuteNonQuery() > 0)
                return true;
            else return false;
            connection.Close();
        }
    }
}
