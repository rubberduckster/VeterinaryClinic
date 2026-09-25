using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeterinaryClinic
{
    internal class TriggerInitializer
    {
        public static void CreateTriggers()
        {
            string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=VeterinaryClinic;Trusted_Connection=True;";

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string sql = @"
                CREATE OR ALTER TRIGGER TR_Appointment_PreventPastDelete
                ON Appointment
                INSTEAD OF DELETE
                AS
                BEGIN
                IF EXISTS (SELECT 1 FROM deleted WHERE DateTime < GETDATE())
                BEGIN THROW 50001, 'Past appointments cannot be deleted.', 1;
                END;

                DELETE FROM Appointment
                WHERE Id IN (SELECT Id FROM deleted);
                END;
            ";

            using SqlCommand command = new SqlCommand(sql, connection);
            command.ExecuteNonQuery();
        }
    }
}
