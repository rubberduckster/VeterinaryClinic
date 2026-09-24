using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace VeterinaryClinic
{
    internal class DatabaseSeeder
    {
        public static void Seed()
        {
            string connectionString = "Server=localhost;Database=VeterinaryClinic;Trusted_Connection=True;TrustServerCertificate=True;";

            using SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            string sql = @"
                INSERT INTO Owner (Name, Phone, Email)
                VALUES ('Anita Hem', '80081355', 'IluvCalCannons@example.com');
            ";

            using SqlCommand command = new SqlCommand(sql, connection);
            command.ExecuteNonQuery();
        }
    }
}
