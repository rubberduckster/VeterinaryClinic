using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeterinaryClinic.Models
{
    internal class Medication
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Dosage { get; set; }
        public string Instructions { get; set; }
        public int AppointmentId { get; set; }
    }
}
