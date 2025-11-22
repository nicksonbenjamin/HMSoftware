using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicApp.Models
{
    [Table("doctor_master")]
    public class DoctorMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("doctor_id")]
        public int DoctorId { get; set; }

        [Required]
        [StringLength(100)]
        [Column("doctor_name")]
        public string DoctorName { get; set; }

        [StringLength(50)]
        public string Degree { get; set; }

        public string Address { get; set; }

        [StringLength(100)]
        public string Place { get; set; }

        [StringLength(50)]
        public string Type { get; set; }

        [StringLength(20)]
        public string PhoneNo { get; set; }

        [StringLength(20)]
        public string MobileNo { get; set; }

        [StringLength(50)]
        public string RegisterNo { get; set; }

        [StringLength(50)]
        public string Section { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        public DateTime? DOB { get; set; }

        public DateTime? DOM { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? RegAmount { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? SecondVisitAmt { get; set; }

        public int? SecondVisitUptoDays { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? RegularFees { get; set; }

        public bool Active { get; set; } = true;

        // Navigation properties
        public ICollection<PatientEntry> PatientEntries { get; set; }
        public ICollection<PatientEntry> ReferredEntries { get; set; }
    }
}
