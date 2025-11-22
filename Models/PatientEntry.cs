using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicApp.Models
{
    [Table("patient_entry")]
    public class PatientEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EntryId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public DateTime RegistrationDate { get; set; }

        [Required]
        public TimeSpan RegistrationTime { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public int ConsultantDoctorId { get; set; }

        public int? RefDoctorId { get; set; }

        [StringLength(100)]
        public string PaymentTerms { get; set; }

        [StringLength(50)]
        public string RegistrationType { get; set; }

        [StringLength(100)]
        public string CompOrInsOrCamp { get; set; }

        public string PatientCondition { get; set; }

        [StringLength(15)]
        public string ReferenceOrPicmeNo { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        // Navigation properties
        [ForeignKey("PatientId")]
        public PatientsMaster Patient { get; set; }

        [ForeignKey("ConsultantDoctorId")]
        public DoctorMaster ConsultantDoctor { get; set; }

        [ForeignKey("RefDoctorId")]
        public DoctorMaster RefDoctor { get; set; }
    }
}
