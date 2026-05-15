using System.Data;
using System.Data.SqlClient;

public class KetNoi
{
    // Use a single backslash in a verbatim string for a named instance:
    public SqlConnection connection = new SqlConnection(@"Server=DESKTOP-E4P638H\TRANDUCANH;Database=QUANLYNHANVIEN;Integrated Security=True;");

    public void CheckConnection()
    {
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
        }
    }
}