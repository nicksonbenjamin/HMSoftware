using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicApp.Models
{
    [Table("patients_new")]
    public class Patient
    {
        [Key]
        [StringLength(36)]
        public string PatientId { get; set; }   // GUID stored as CHAR(36)

        [Required, StringLength(50)]
        public string UHIDType { get; set; }

        [StringLength(50)]
        public string UHIDNo { get; set; }

        public DateTime RegistrationDate { get; set; }
        public TimeSpan RegistrationTime { get; set; }

        [Required, StringLength(3)]
        public string Sex { get; set; }

        [Required, StringLength(10)]
        public string PatientTitle { get; set; }

        [Required, StringLength(100)]
        public string PatientName { get; set; }

        public DateTime? DOB { get; set; }

        public int Age { get; set; }

        [StringLength(10)]
        public string GuardianTitle { get; set; }

        [StringLength(100)]
        public string Guardian { get; set; }

        public string Address { get; set; }

        [StringLength(100)]
        public string Place { get; set; }

        [StringLength(100)]
        public string District { get; set; }

        [StringLength(100)]
        public string GSTState { get; set; }

        [StringLength(100)]
        public string Country { get; set; }

        [StringLength(10)]
        public string PinCode { get; set; }

        [StringLength(12)]
        public string PatientAadhar { get; set; }

        [StringLength(12)]
        public string GuardianAadhar { get; set; }

        [StringLength(15)]
        public string MobileNo { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        public byte[] Photo { get; set; }

        [StringLength(100)]
        public string Occupation { get; set; }

        [StringLength(50)]
        public string MaritalStatus { get; set; }

        [StringLength(10)]
        public string BloodGroup { get; set; }

        public string AllergicTo { get; set; }

        [StringLength(100)]
        public string ConsultantDoctor { get; set; }

        [StringLength(100)]
        public string RefDoctor { get; set; }

        [StringLength(100)]
        public string PaymentTerms { get; set; }

        [StringLength(50)]
        public string RegistrationType { get; set; }

        [StringLength(100)]
        public string CompOrInsOrCamp { get; set; }

        public string PatientCondition { get; set; }

        [StringLength(15)]
        public string ReferenceOrPicmeNo { get; set; }

        public bool IsActive { get; set; }

        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
