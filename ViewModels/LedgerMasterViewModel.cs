using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ClinicApp.ViewModels
{
  public class LedgerMasterViewModel
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Ledger Type")]
    public string LedgerType { get; set; }  // "Company", "Supplier", "Customer"

    [Required]
    [Display(Name = "Ledger Name")]
    public string LedgerName { get; set; }

    public string Address { get; set; }

    public string Place { get; set; }

    [Display(Name = "GSTIN")]
    public string GSTIN { get; set; }

    [Display(Name = "GST State")]
    public string GSTState { get; set; }  // e.g., "33-Tamil Nadu"

    [Display(Name = "Credit Days")]
    [Range(0, 365)]
    public int CreditDays { get; set; } = 0;

    [Display(Name = "Contact Person")]
    public string ContactPerson { get; set; }

    [Display(Name = "Phone No")]
    [Phone]
    public string PhoneNo { get; set; }

    [Display(Name = "Mobile No")]
    [Phone]
    public string MobileNo { get; set; }

    [EmailAddress]
    public string Email { get; set; }

    [Display(Name = "Drug License No")]
    public string DrugLicenseNo { get; set; }

    public string Remarks { get; set; }

    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;

    [Display(Name = "Created At")]
    public DateTime CreatedAt { get; set; }

    [Display(Name = "Updated At")]
        public DateTime UpdatedAt { get; set; }
    
    public string ErrorMessage { get; set; }
}
}