using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClinicApp.Models
{
    public class Prescription
    {
        [Key]
        public int PrescriptionId { get; set; }

        [Display(Name = "Entry Type")]
        [Required]
        public string EntryType { get; set; }

        [Display(Name = "Entry No")]
        public string EntryNo { get; set; }

        [Display(Name = "Entry Date")]
        [DataType(DataType.DateTime)]
        public DateTime EntryDate { get; set; }

        public string UHID { get; set; }

        [Display(Name = "Patient Name")]
        [Required]
        public string PatientName { get; set; }

        public string Address { get; set; }
        public string Place { get; set; }

        [Display(Name = "Cell No")]
        public string CellNo { get; set; }

        public string Sex { get; set; }
        public int? Age { get; set; }

        [Display(Name = "Blood Group")]
        public string BloodGroup { get; set; }

        [Display(Name = "Height (cm)")]
        public decimal? HeightCm { get; set; }

        [Display(Name = "Weight (kg)")]
        public decimal? WeightKg { get; set; }

        public decimal? BMI { get; set; }
        public decimal? Temperature { get; set; }
        public string BP { get; set; }
        public decimal? SPO2 { get; set; }

        [Display(Name = "Pulse Rate")]
        public int? PulseRate { get; set; }

        [Display(Name = "Consult By")]
        public string ConsultBy { get; set; }

        [Display(Name = "Referred By")]
        public string ReferredBy { get; set; }

        [Display(Name = "Disease / Diagnosis")]
        public string Disease { get; set; }

        public string Remarks { get; set; }

        [Display(Name = "Total Amount")]
        [DataType(DataType.Currency)]
        public decimal? TotalAmount { get; set; }

        [Display(Name = "Next Visit After (Days)")]
        public int? NextVisitAfter { get; set; }

        [Display(Name = "Next Visit Date")]
        [DataType(DataType.Date)]
        public DateTime? NextVisitDate { get; set; }

        // One-to-Many relationship with details
        public List<PrescriptionDetail> Details { get; set; } = new List<PrescriptionDetail>();
    }
}
