using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ClinicApp.Models; // adjust namespace as per your project

namespace ClinicApp.ViewModels
{
    public class PrescriptionViewModel
    {
        // --- Basic Prescription Info ---
        public int? PrescriptionId { get; set; }

        [Required]
        [Display(Name = "Entry Type")]
        public string EntryType { get; set; }

        [Display(Name = "Entry No")]
        public string EntryNo { get; set; }

        [Display(Name = "Entry Date")]
        [DataType(DataType.DateTime)]
        public DateTime? EntryDate { get; set; } = DateTime.Now;

        [Display(Name = "UHID")]
        public string UHID { get; set; }

        [Required]
        [Display(Name = "Patient Name")]
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


        // --- Detail Items (child table) ---
        public List<PrescriptionDetailViewModel> Details { get; set; } = new List<PrescriptionDetailViewModel>();

    public string ErrorMessage { get; set; }
        // --- Dropdown / Lookup Data for UI ---
        public List<string> DiseaseList { get; set; } = new List<string>
        {
            "ABDOMEN PAIN",
            "ACCIDENT",
            "ADMISSION",
            "ALLERGIES",
            "ANGINA PECTORIS",
            "ANTENATAL FIRST TRIMESTER"
        };

        public List<string> DoctorList { get; set; } = new List<string>
        {
            "A.MICHAEL, MS, MCH",
            "B.JOHNSON, MD",
            "C.SMITH, MBBS"
        };
    }
}
