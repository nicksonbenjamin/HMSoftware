using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicApp.Models
{
    public class LedgerMaster
    {
         public int Id { get; set; }

    public string LedgerType { get; set; }  // "Company", "Supplier", "Customer"


    public string LedgerName { get; set; }

    public string Address { get; set; }

    public string Place { get; set; }

    public string GSTIN { get; set; }
   
    public string GSTState { get; set; }  // e.g., "33-Tamil Nadu"
     
    public int CreditDays { get; set; } = 0;

     public string ContactPerson { get; set; }

    public string PhoneNo { get; set; }

    public string MobileNo { get; set; }

    public string Email { get; set; }
    public string DrugLicenseNo { get; set; }

    public string Remarks { get; set; }

      public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}