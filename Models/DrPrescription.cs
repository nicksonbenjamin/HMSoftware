using System;

namespace ClinicApp.Models
{
    public class DrPrescription
    {
        public int DrPrescriptionId { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public DateTime PrescriptionDate { get; set; }
        public string EntryType { get; set; }
        public int EntryNumber { get; set; }
        public string EntryPeriod { get; set; }
        public int DeseaseId { get; set; }

            public string DeseaseName { get; set; }

            
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public decimal BMI { get; set; }
        public decimal TempInCelcius { get; set; }
        public string BP { get; set; }
        public string SPO2 { get; set; }
        public decimal PulseRate { get; set; }
        public DateTime? NextVisitDate { get; set; }

        // -----------------------
        // Doctor IDs
        // -----------------------
        public int ConsultantDoctorId { get; set; }
        public int? RefDoctorId { get; set; }
    }
}
