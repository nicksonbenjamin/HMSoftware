using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClinicApp.ViewModels
{
    // Simple domain model (replace with your real model or remove if you have one)
    public class VisitViewModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid PatientId { get; set; }
        public string PatientName { get; set; }
        public Guid DoctorId { get; set; }
        public string DoctorName { get; set; }
        public DateTime VisitDate { get; set; } = DateTime.Now;
        public string Reason { get; set; }
        public string Notes { get; set; }
        public bool IsFollowUp { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }

}