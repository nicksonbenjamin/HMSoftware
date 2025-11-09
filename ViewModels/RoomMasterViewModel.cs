using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ClinicApp.Models; // adjust namespace as per your project
namespace ClinicApp.ViewModels
{
    public class RoomMasterViewModel
    {
        public int RoomId { get; set; }

        [Required(ErrorMessage = "Room No is required.")]
        [StringLength(10)]
        [Display(Name = "Room No")]
        public string RoomNo { get; set; }

        [StringLength(10)]
        [Display(Name = "Floor No")]
        public string FloorNo { get; set; }

        [Required(ErrorMessage = "Please select room type.")]
        [Display(Name = "Room Type")]
        public int TypeId { get; set; }

        public string RoomTypeName { get; set; }

        [Display(Name = "No. of Beds")]
        public int? NoOfBeds { get; set; }

        [Display(Name = "Rent per Day")]
        [DataType(DataType.Currency)]
        public decimal? RentPerDay { get; set; }

        [Display(Name = "Rent per Hour")]
        [DataType(DataType.Currency)]
        public decimal? RentPerHour { get; set; }

        [Display(Name = "Nursing Charge/Day")]
        [DataType(DataType.Currency)]
        public decimal? NursingChargePerDay { get; set; }

        [StringLength(255)]
        public string Remarks { get; set; }

        [Display(Name = "Other Charge Description")]
        [StringLength(100)]
        public string ChargeDescription { get; set; }

        [Display(Name = "Amount per Day")]
        [DataType(DataType.Currency)]
        public decimal? AmountPerDay { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        // Dropdown list for room types
        public IEnumerable<RoomTypeMaster> RoomTypeList { get; set; }
    }
}
