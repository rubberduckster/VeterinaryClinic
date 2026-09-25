using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeterinaryClinic.Helpers
{
    internal static class DatabaseConnection
    {
        public static string ConnectionString { get; private set; } = "";

        public static void LoginAsUser()
        {
            ConnectionString ="Server=localhost;Database=VeterinaryClinic;User Id=clinicUser;Password=U53R11111;TrustServerCertificate=True;";
        }

        public static void LoginAsAdmin()
        {
            ConnectionString ="Server=localhost;Database=VeterinaryClinic;User Id=clinicAdmin;Password=4DM1N11111;TrustServerCertificate=True;";
        }
    }
}
