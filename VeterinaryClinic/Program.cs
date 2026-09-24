namespace VeterinaryClinic
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Only run if no database
            //DatabaseInitializer.CreateDatabase();

            DatabaseSeeder.Seed();
        }
    }
}
