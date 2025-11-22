using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClinicApp.ViewModels
{
    public class PatientRegistrationViewModel
    {
        // Primary key
        public int PatientId { get; set; }  

        // Patient Master Fields
        [Required]
        [Display(Name = "UHID Type")]
        public string UHIDType { get; set; }
        public List<SelectListItem> UHIDTypes { get; set; } = new List<SelectListItem>();

        [Display(Name = "UHID No")]
        public string UHIDNo { get; set; }

        [Required]
        [Display(Name = "Registration Date")]
        [DataType(DataType.Date)]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Registration Time")]
        [DataType(DataType.Time)]
        public TimeSpan RegistrationTime { get; set; } = DateTime.Now.TimeOfDay;

        [Required]
        [Display(Name = "Sex")]
        public string Sex { get; set; }
        public List<SelectListItem> Sexes { get; set; } = new List<SelectListItem>();

        [Required]
        [Display(Name = "Patient Title")]
        public string PatientTitle { get; set; }
        public List<SelectListItem> PatientTitles { get; set; } = new List<SelectListItem>();

        [Required]
        [Display(Name = "Patient Name")]
        public string PatientName { get; set; }

        [Display(Name = "DOB")]
        [DataType(DataType.Date)]
        public DateTime? DOB { get; set; }

        [Required]
        [Display(Name = "Age")]
        public int Age { get; set; }

        [Required]
        [Display(Name = "Guardian Title")]
        public string GuardianTitle { get; set; }
        public List<SelectListItem> GuardianTitles { get; set; } = new List<SelectListItem>();

        [Required]
        [Display(Name = "Guardian")]
        public string Guardian { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Place")]
        public string Place { get; set; }

        [Display(Name = "District")]
        public string District { get; set; }

        [Required]
        [Display(Name = "GST State")]
        public string GSTState { get; set; }
        public List<SelectListItem> GSTStates { get; set; } = new List<SelectListItem>();

        [Required]
        [Display(Name = "Country")]
        public string Country { get; set; }
        public List<SelectListItem> Countries { get; set; } = new List<SelectListItem>();

        [Display(Name = "Pin Code")]
        public string PinCode { get; set; }

        [Display(Name = "Patient Aadhar")]
        public string PatientAadhar { get; set; }

        [Display(Name = "Guardian Aadhar")]
        public string GuardianAadhar { get; set; }

        [Required]
        [Display(Name = "Mobile No")]
        [Phone]
        public string MobileNo { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public IFormFile Photo { get; set; }

        [Display(Name = "Occupation")]
        public string Occupation { get; set; }

        [Display(Name = "Marital Status")]
        public string MaritalStatus { get; set; }
        public List<SelectListItem> MaritalStatuses { get; set; } = new List<SelectListItem>();

        [Display(Name = "Blood Group")]
        public string BloodGroup { get; set; }
        public List<SelectListItem> BloodGroups { get; set; } = new List<SelectListItem>();

        [Display(Name = "Allergic To")]
        public string AllergicTo { get; set; }

        [Required]
        [Display(Name = "Consultant Doctor")]
        public int ConsultantDoctorId { get; set; }
        public List<SelectListItem> ConsultantDoctors { get; set; } = new List<SelectListItem>();

        [Display(Name = "Referred By")]
        public int? RefDoctorId { get; set; }
        public List<SelectListItem> RefDoctors { get; set; } = new List<SelectListItem>();

        [Display(Name = "Payment Terms")]
        public string PaymentTerms { get; set; }
        public List<SelectListItem> PaymentTermsList { get; set; } = new List<SelectListItem>();

        [Display(Name = "Registration Type")]
        public string RegistrationType { get; set; }
        public List<SelectListItem> RegistrationTypes { get; set; } = new List<SelectListItem>();

        [Display(Name = "Comp/Ins/Camp")]
        public string CompOrInsOrCamp { get; set; }
        public List<SelectListItem> CompInsCampList { get; set; } = new List<SelectListItem>();

        [Display(Name = "Patient Condition")]
        public string PatientCondition { get; set; }

        [Display(Name = "Reference/PICME No")]
        public string ReferenceOrPicmeNo { get; set; }

        public bool IsActive { get; set; } = true;

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Updated By")]
        public string UpdatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
