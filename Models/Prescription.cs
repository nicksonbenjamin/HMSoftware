using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicApp.Models
{
    [Table("prescription_master")]
    public class Prescription
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PrescriptionId { get; set; }

        // FK
        [Required, StringLength(36)]
        public string PatientId { get; set; }

        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }

        // Header
        [Required, StringLength(50)]
        public string EntryType { get; set; }

        [StringLength(50)]
        public string EntryNo { get; set; }

        public DateTime EntryDate { get; set; }

        // Patient snapshot
        [StringLength(50)]
        public string UHIDType { get; set; }

        [StringLength(50)]
        public string UHIDNo { get; set; }

        [Required, StringLength(100)]
        public string PatientName { get; set; }

        public string Address { get; set; }
        public string Place { get; set; }

        [StringLength(15)]
        public string CellNo { get; set; }

        [StringLength(3)]
        public string Sex { get; set; }

        public int? Age { get; set; }

        [StringLength(10)]
        public string BloodGroup { get; set; }

        // Vitals
        public decimal? HeightCm { get; set; }
        public decimal? WeightKg { get; set; }
        public decimal? BMI { get; set; }
        public decimal? Temperature { get; set; }

        [StringLength(20)]
        public string BP { get; set; }

        public int? SPO2 { get; set; }
        public int? PulseRate { get; set; }

        // Consultation
        [StringLength(100)]
        public string ConsultBy { get; set; }

        [StringLength(100)]
        public string ReferredBy { get; set; }

        [StringLength(255)]
        public string Disease { get; set; }

        public string Remarks { get; set; }

        // Billing
        public decimal? TotalAmount { get; set; }
        public int? NextVisitAfter { get; set; }
        public DateTime? NextVisitDate { get; set; }

        // Children tables
        public ICollection<PrescriptionDetail> Medicines { get; set; }
        public ICollection<PrescriptionClinicalDetail> ClinicalDetails { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
