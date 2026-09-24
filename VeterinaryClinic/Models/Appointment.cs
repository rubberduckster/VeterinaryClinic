using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace VeterinaryClinic.Models
{
    internal class Appointment
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Purpose { get; set; }
        public int VeterinarianId { get; set; }
        public int JournalId { get; set; }
    }
}
