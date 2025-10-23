// Models/Patient.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClinicApp.ViewModels
{
    public class PatientViewModel
    {
        public string PatientId { get; set; }
        public string UHIDType { get; set; }
        public string UHIDNo { get; set; }
        public DateTime RegistrationDate { get; set; }
        public TimeSpan RegistrationTime { get; set; }
        public string Sex { get; set; }
        public string PatientTitle { get; set; }
        public string PatientName { get; set; }
        public DateTime? DOB { get; set; }
        public int Age { get; set; }
        public string GuardianTitle { get; set; }
        public string Guardian { get; set; }
        public string Address { get; set; }
        public string Place { get; set; }
        public string District { get; set; }
        public string GSTState { get; set; }
        public string Country { get; set; }
        public string PinCode { get; set; }
        public string PatientAadhar { get; set; }
        public string GuardianAadhar { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public byte[] Photo { get; set; }
        public string Occupation { get; set; }
        public string MaritalStatus { get; set; }
        public string BloodGroup { get; set; }
        public string AllergicTo { get; set; }
        public string ConsultantDoctor { get; set; }
        public string RefDoctor { get; set; }
        public string PaymentTerms { get; set; }
        public string RegistrationType { get; set; }
        public string CompOrInsOrCamp { get; set; }
        public string PatientCondition { get; set; }
        public string ReferenceOrPicmeNo { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string ErrorMessage { get; set; }
    }
}