using System;
using System.ComponentModel.DataAnnotations;

namespace ClinicApp.Models
{
    public class PrescriptionMedicine
    {
        [Key]
        public Guid MedicineId { get; set; }

        public Guid PrescriptionId { get; set; }
        public Prescription Prescription { get; set; }

        public string MedicineName { get; set; }
        public string DosePattern { get; set; }   // e.g. "1-0-1"
        public string UsageInstruction { get; set; } // "After Food"
        public int Days { get; set; }
        public int Quantity { get; set; }
        public string Note { get; set; }
    }
}
