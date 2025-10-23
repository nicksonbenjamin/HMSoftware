using System;

namespace ClinicApp.ViewModels
{
    public class AppointmentViewModel
    {
        public int AppointmentId { get; set; }
        public Guid PatientId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
    }
}
