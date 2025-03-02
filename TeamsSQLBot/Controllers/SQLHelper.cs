using System;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

public class SQLHelper
{
    private const string connectionString = "";

    public async Task<string> ExecuteSQLQuery(string sqlQuery)
    {
        if (string.IsNullOrWhiteSpace(sqlQuery))
        {
            throw new ArgumentException("La query SQL non può essere vuota.", nameof(sqlQuery));
        }

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            await conn.OpenAsync();
            using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
            {
                var result = await cmd.ExecuteScalarAsync();
                return result?.ToString() ?? "Nessun risultato trovato.";
            }
        }
    }
}
