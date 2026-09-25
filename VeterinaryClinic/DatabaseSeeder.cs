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

            // Owner
            string sql = @"
                INSERT INTO Owner (Name, Phone, Email)
                VALUES ('Anita Hem', '80081355', 'IluvCalCannons@example.com');

                SELECT SCOPE_IDENTITY();
            ";

            // Animal
            using SqlCommand ownerCommand = new SqlCommand(sql, connection);
            int ownerId = Convert.ToInt32(ownerCommand.ExecuteScalar());

            string animalSql = @"
                INSERT INTO Animal (OwnerId, Name, DateOfBirth, Species, Breed)
                VALUES (@OwnerId, 'Bucket', '2021-05-12', 'Cow', 'Holstein');

                SELECT SCOPE_IDENTITY();
            ";

            using SqlCommand animalCommand = new SqlCommand(animalSql, connection);
            animalCommand.Parameters.AddWithValue("@OwnerId", ownerId);
            int animalId = Convert.ToInt32(animalCommand.ExecuteScalar());

            // Health Journal
            string journalSql = @"
                INSERT INTO HealthJournal (AnimalId)
                VALUES (@AnimalId);

                SELECT SCOPE_IDENTITY();
            ";

            using SqlCommand journalCommand = new SqlCommand(journalSql, connection);
            journalCommand.Parameters.AddWithValue("@AnimalId", animalId);
            int journalId = Convert.ToInt32(journalCommand.ExecuteScalar());

            // Veterinarian
            string veterinarianSql = @"
                INSERT INTO Veterinarian (Name)
                VALUES ('Dr. Cowspector');

                SELECT SCOPE_IDENTITY();
            ";

            using SqlCommand veterinarianCommand = new SqlCommand(veterinarianSql, connection);
            int veterinarianId = Convert.ToInt32(veterinarianCommand.ExecuteScalar());

            // Appointment
            string appointmentSql = @"
                INSERT INTO Appointment (JournalId, VeterinarianId, DateTime, Purpose)
                VALUES (@JournalId, @VeterinarianId, '2026-09-20 10:30', 'General checkup');

                SELECT SCOPE_IDENTITY();
            ";

            using SqlCommand appointmentCommand = new SqlCommand(appointmentSql, connection);

            appointmentCommand.Parameters.AddWithValue("@JournalId", journalId);
            appointmentCommand.Parameters.AddWithValue("@VeterinarianId", veterinarianId);

            int appointmentId = Convert.ToInt32(appointmentCommand.ExecuteScalar());

            // Treatment
            string treatmentSql = @"
                INSERT INTO Treatment (AppointmentId, Description, Notes)
                VALUES (@AppointmentId, 'Ear cleaning', 'Ear cleaned during appointment');
            ";

            using SqlCommand treatmentCommand = new SqlCommand(treatmentSql, connection);

            treatmentCommand.Parameters.AddWithValue("@AppointmentId", appointmentId);
            treatmentCommand.ExecuteNonQuery();

            // Medication
            string medicationSql = @"
                INSERT INTO Medication (AppointmentId, Name, Dosage, Instructions)
                VALUES (@AppointmentId, 'Ear Drops', '2 drops', 'Apply twice daily for 7 days');
            ";

            using SqlCommand medicationCommand = new SqlCommand(medicationSql, connection);

            medicationCommand.Parameters.AddWithValue("@AppointmentId", appointmentId);
            medicationCommand.ExecuteNonQuery();
        }
    }
}
