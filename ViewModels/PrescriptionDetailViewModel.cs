using System;
using System.ComponentModel.DataAnnotations;

namespace ClinicApp.ViewModels
{
    public class PrescriptionDetailViewModel
    {
        [Key]
        public int DetailId { get; set; }

        [Required(ErrorMessage = "Prescription ID is required.")]
        public int PrescriptionDetailId  { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [Display(Name = "Medicine / Instruction Description")]
        public string Description { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Amount")]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be a positive value.")]
        public decimal? Amount { get; set; }

        // Optional: If you want to display the Prescription info in the detail view
        public string PrescriptionName { get; set; } 

        // Error message for UI (as used in your PatientsController pattern)
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
