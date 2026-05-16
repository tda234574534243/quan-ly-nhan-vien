using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    internal class DAL_Audit : KetNoi
    {
        public void WriteAudit(string eventType, string username, string target, string details)
        {
            try
            {
                if (connection.State != ConnectionState.Open) connection.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_AuditLog_Add", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EventType", eventType ?? string.Empty);
                    cmd.Parameters.AddWithValue("@Username", username ?? string.Empty);
                    cmd.Parameters.AddWithValue("@Target", target ?? string.Empty);
                    cmd.Parameters.AddWithValue("@Details", details ?? string.Empty);
                    cmd.ExecuteNonQuery();
                }
            }
            catch
            {
                // swallow to avoid breaking main flow
            }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();
            }
        }
    }
}
