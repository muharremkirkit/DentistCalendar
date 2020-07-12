using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentistCalendar.Models
{
    public class AddOrUpdateAppointmentViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string PatientName { get; set; }
        public string PatientSurName { get; set; }
        public string Description { get; set; }
    }
}
