using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeterinaryClinic.Models
{
    internal class Treatment
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public int AppointmentId { get; set; }
    }
}
