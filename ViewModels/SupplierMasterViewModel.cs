using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClinicApp.ViewModels
{
    public class SupplierMasterViewModel
    {
        // Supplier Fields
        public int SupplierId { get; set; }

        public string SupplierName { get; set; }

        public string ContactPerson { get; set; }

        public string MobileNo { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string GSTNo { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


        // ============================
        // EXTRA VIEW PROPERTIES
        // ============================

        // Active / Inactive Dropdown
        public IEnumerable<SelectListItem> ActiveStatusList { get; set; }

        // If you want to show products supplied by this supplier
        public IEnumerable<SelectListItem> ProductList { get; set; }

        // For multi-select if needed
        public List<int> SelectedProducts { get; set; }

        // Common dropdown for GST state codes or GST types
        public IEnumerable<SelectListItem> GSTTypeList { get; set; }

        // For country / state dropdown
        public IEnumerable<SelectListItem> CountryList { get; set; }
        public IEnumerable<SelectListItem> StateList { get; set; }

        // Extra notes or remarks
        public string Remarks { get; set; }
    }
}
