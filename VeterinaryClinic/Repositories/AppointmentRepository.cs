using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinaryClinic.Helpers;

namespace VeterinaryClinic.Repositories
{
    internal class AppointmentRepository
    {
        public int CreateAppointment(int journalId, int veterinarianId, DateTime dateTime, string purpose)
        {
            using SqlConnection connection = new SqlConnection(DatabaseConnection.ConnectionString);
            connection.Open();

            string sql = @"
                INSERT INTO Appointment (JournalId, VeterinarianId, DateTime, Purpose)
                VALUES (@JournalId, @VeterinarianId, @DateTime, @Purpose);
                SELECT SCOPE_IDENTITY();
            ";

            using SqlCommand command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@JournalId", journalId);
            command.Parameters.AddWithValue("@VeterinarianId", veterinarianId);
            command.Parameters.AddWithValue("@DateTime", dateTime);
            command.Parameters.AddWithValue("@Purpose", purpose);

            int appointmentId = Convert.ToInt32(command.ExecuteScalar());

            return appointmentId;
        }

        public void GetAnimalMedicalHistory(string animalName)
        {
            using SqlConnection connection = new SqlConnection(DatabaseConnection.ConnectionString);
            connection.Open();

            using SqlCommand command = new SqlCommand("GetAnimalMedicalHistory", connection);

            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@AnimalName", animalName);

            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine($"Date: {reader["DateTime"]}, " + $"Veterinarian: {reader["VeterinarianName"]}, " + $"Purpose: {reader["Purpose"]}");
            }
        }

        public DateTime GetAppointmentDateTime(int appointmentId)
        {
            using SqlConnection connection = new SqlConnection(DatabaseConnection.ConnectionString);
            connection.Open();

            string sql = @"
                SELECT DateTime
                FROM Appointment
                WHERE Id = @AppointmentId;
            ";

            using SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@AppointmentId", appointmentId);

            DateTime oldDateTime = Convert.ToDateTime(command.ExecuteScalar());

            return oldDateTime;
        }

        public void UpdateAppointment(int appointmentId, DateTime newDateTime)
        {
            using SqlConnection connection = new SqlConnection(DatabaseConnection.ConnectionString);
            connection.Open();

            string sql = @"
                UPDATE Appointment
                SET DateTime = @DateTime
                WHERE Id = @AppointmentId;
            ";

            using SqlCommand command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@DateTime", newDateTime);
            command.Parameters.AddWithValue("@AppointmentId", appointmentId);

            command.ExecuteNonQuery();
        }

        public bool AnimalHasMedicalHistory(int animalId)
        {
            using SqlConnection connection = new SqlConnection(DatabaseConnection.ConnectionString);
            connection.Open();

            string sql = @"
                SELECT COUNT(*)
                FROM Appointment
                INNER JOIN HealthJournal
                ON Appointment.JournalId = HealthJournal.Id
                WHERE HealthJournal.AnimalId = @AnimalId;
            ";

            using SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@AnimalId", animalId);

            int appointmentCount = Convert.ToInt32(command.ExecuteScalar());

            return appointmentCount > 0;
        }

        public int GetJournalIdByAnimalName(string animalName)
        {
            using SqlConnection connection = new SqlConnection(DatabaseConnection.ConnectionString);
            connection.Open();

            string sql = @"
                SELECT HealthJournal.Id
                FROM HealthJournal
                INNER JOIN Animal
                ON HealthJournal.AnimalId = Animal.Id
                WHERE Animal.Name = @AnimalName;
            ";

            using SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@AnimalName", animalName);

            object? result = command.ExecuteScalar();

            if (result == null)
            {
                throw new InvalidOperationException("Animal not found.");
            }

            return Convert.ToInt32(result);
        }

        public string GetAnimalData(int animalId)
        {
            using SqlConnection connection = new SqlConnection(DatabaseConnection.ConnectionString);
            connection.Open();

            string sql = @"
                SELECT Name, Species, Breed
                FROM Animal
                WHERE Id = @AnimalId;
            ";

            using SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@AnimalId", animalId);

            using SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                return $"Name: {reader["Name"]}, Species: {reader["Species"]}, Breed: {reader["Breed"]}";
            }

            return "Animal not found";
        }

        public void ShowAnimal(string animalName)
        {
            using SqlConnection connection = new SqlConnection(DatabaseConnection.ConnectionString);
            connection.Open();

            string sql = @"
            SELECT
            Animal.Name AS AnimalName,
            Animal.Species,
            Animal.Breed,
            Animal.DateOfBirth,
            Owner.Name AS OwnerName,
            Owner.Phone,
            Owner.Email
            FROM Animal
            INNER JOIN Owner
            ON Animal.OwnerId = Owner.Id
            WHERE Animal.Name = @AnimalName;
            ";

            using SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@AnimalName", animalName);

            using SqlDataReader reader = command.ExecuteReader();

            if (!reader.Read())
            {
                Console.WriteLine("Animal not found.");
                return;
            }

            Console.WriteLine("\n=== Animal ===");
            Console.WriteLine($"Name: {reader["AnimalName"]}");
            Console.WriteLine($"Species: {reader["Species"]}");
            Console.WriteLine($"Breed: {reader["Breed"]}");
            Console.WriteLine($"Date of birth: {reader["DateOfBirth"]}");
            Console.WriteLine($"Owner: {reader["OwnerName"]}");

            Console.WriteLine("\n1. View owner information");
            Console.WriteLine("2. View medical history");
            Console.WriteLine("0. Return");
            Console.Write("Choose: ");

            string? choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.WriteLine("\n=== Owner ===");
                Console.WriteLine($"Name: {reader["OwnerName"]}");
                Console.WriteLine($"Phone: {reader["Phone"]}");
                Console.WriteLine($"Email: {reader["Email"]}");
            }
            else if (choice == "2")
            {
                GetAnimalMedicalHistory(animalName);
            }
            Console.WriteLine($"Email: {reader["Email"]}");
            }

        public void DeleteAnimal(int animalId)
        {
            using SqlConnection connection = new SqlConnection(DatabaseConnection.ConnectionString);
            connection.Open();

            string sql = @"
                DELETE FROM HealthJournal
                WHERE AnimalId = @AnimalId;

                DELETE FROM Animal
                WHERE Id = @AnimalId;
            ";

            using SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@AnimalId", animalId);

            command.ExecuteNonQuery();
        }

        public int GetVeterinarianIdByName(string veterinarianName)
        {
            using SqlConnection connection = new SqlConnection(DatabaseConnection.ConnectionString);
            connection.Open();

            string sql = @"
                SELECT Id
                FROM Veterinarian
                WHERE Name = @VeterinarianName;
            ";

            using SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@VeterinarianName", veterinarianName);

            object? result = command.ExecuteScalar();

            if (result == null)
            {
                throw new InvalidOperationException("Veterinarian not found.");
            }

            return Convert.ToInt32(result);
        }
    }
}
