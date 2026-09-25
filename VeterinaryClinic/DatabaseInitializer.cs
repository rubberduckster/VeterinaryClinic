using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace VeterinaryClinic
{
    internal class DatabaseInitializer
    {
        public static void CreateDatabase()
        {
            string connectionString = "Server=localhost;Database=master;Trusted_Connection=True;TrustServerCertificate=True;";

            string script = File.ReadAllText("VeterinaryClinic.sql");

            using SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            // Makes GO readable
            string[] batches = script.Split(new[] { "\r\nGO\r\n", "\nGO\n" },StringSplitOptions.RemoveEmptyEntries);

            foreach (string batch in batches)
            {
                using SqlCommand command = new SqlCommand(batch, connection);
                command.ExecuteNonQuery();
            }
        }
    }
}
