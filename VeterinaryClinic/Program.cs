using VeterinaryClinic.Helpers;
using VeterinaryClinic.Services;

namespace VeterinaryClinic
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Create database // First time only
            // DatabaseInitializer.CreateDatabase();

            // Seed data
            // DatabaseSeeder.Seed();

            // Exception test
            // AppointmentService appointmentService = new AppointmentService();
            // appointmentService.CreateAppointment(1,1,new DateTime(2020, 1, 1),"Test appointment");

            // TriggerInitializer.CreateTriggers();

            // Log data test!
            // AppointmentService appointmentService = new AppointmentService();
            // appointmentService.CreateAppointment(1, 1, new DateTime(2026, 10, 10, 10, 30, 0), "Logging test");

            Console.WriteLine("Login");
            Console.WriteLine("1. User");
            Console.WriteLine("2. Admin");
            Console.Write("Choose: ");

            string? loginChoice = Console.ReadLine();

            if (loginChoice == "2")
            {
                DatabaseConnection.LoginAsAdmin();
                Console.WriteLine("Logged in as admin.");
            }
            else
            {
                DatabaseConnection.LoginAsUser();
                Console.WriteLine("Logged in as user.");
            }

            AppointmentService appointmentService = new AppointmentService();

            bool running = true;

            while (running)
            {
                Console.WriteLine("\nVeterinary Clinic");
                Console.WriteLine("1. Book appointment");
                Console.WriteLine("2. Find animal");
                Console.WriteLine("3. Delete animal");
                Console.WriteLine("0. Exit");
                Console.Write("Choose: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        Console.WriteLine("Book appointment");
                        BookAppointment();
                        break;

                    case "2":
                        Console.Clear();
                        Console.WriteLine("Find animal");
                        FindAnimal();
                        break;

                    case "3":
                        DeleteAnimal();
                        break;

                    case "0":
                        running = false;
                        break;

                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }

            void BookAppointment()
            {
                Console.Write("\nAnimal name: ");
                string animalName = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(animalName))
                {
                    Console.WriteLine("Animal name cannot be empty.");
                    return;
                }

                Console.Write("Veterinarian name: ");
                string veterinarianName = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(veterinarianName))
                {
                    Console.WriteLine("Veterinarian name cannot be empty.");
                    return;
                }

                Console.Write("Date and time (Day/Month/Year Hour:Minute): ");
                string dateInput = Console.ReadLine() ?? "";

                if (!DateTime.TryParseExact(dateInput, "dd/MM/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime dateTime))
                {
                    Console.WriteLine("Invalid date. Use format: dd/MM/yyyy HH:mm");
                    return;
                }

                Console.Write("Purpose: ");
                string purpose = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(purpose))
                {
                    Console.WriteLine("Purpose cannot be empty.");
                    return;
                }

                appointmentService.CreateAppointment(animalName, veterinarianName, dateTime, purpose);

                Console.WriteLine("Appointment booked!");
            }

            void FindAnimal()
            {
                Console.Write("\nAnimal name: ");
                string animalName = Console.ReadLine() ?? "";

                appointmentService.ShowAnimal(animalName);
            }

            void DeleteAnimal()
            {
                Console.Write("\nAnimal ID: ");

                if (!int.TryParse(Console.ReadLine(), out int animalId))
                {
                    Console.WriteLine("Invalid animal ID.");
                    return;
                }

                try
                {
                    appointmentService.DeleteAnimal(animalId);
                    Console.WriteLine("Animal deleted.");
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (Microsoft.Data.SqlClient.SqlException)
                {
                    Console.WriteLine("You do not have permission to delete this animal.");
                }
            }
        }
    }
}
