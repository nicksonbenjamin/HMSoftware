// Models/AttendingStaff.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClinicApp.Models
{
    public class AttendingStaff
    {
        [Key]
        public int StaffID { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<Visit> Visits { get; set; }
    }
}