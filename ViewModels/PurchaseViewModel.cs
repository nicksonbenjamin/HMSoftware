using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace ClinicApp.ViewModels
{
    public class PurchaseViewModel
    {
        public int PurchaseId { get; set; }
        public string PurchaseNo { get; set; }
        public DateTime PurchaseDate { get; set; }

        public int SupplierId { get; set; }
        public string SupplierName { get; set; } // Added

        public string SupplierInvoiceNo { get; set; } // Added
        public DateTime? SupplierInvoiceDate { get; set; } // Added

        public string Remarks { get; set; }

        public decimal TotalQty { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalTax { get; set; }
        public decimal RoundOff { get; set; }
        public decimal NetAmount { get; set; }

        // Dropdowns
        public List<SelectListItem> SupplierList { get; set; } = new(); // Added
        public List<SelectListItem> ProductList { get; set; } = new(); // Added

        // Purchase items
        public List<PurchaseItemViewModel> Items { get; set; } = new();
    }

    public class PurchaseItemViewModel
    {
        public int PurchaseItemId { get; set; } // Added
        public int ProductId { get; set; }
        public string ProductName { get; set; } // Added
        public string BatchNo { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string UOM { get; set; }  // Unit of Measure
        public decimal Qty { get; set; }
        public decimal FreeQty { get; set; }
        public decimal Rate { get; set; }
        public decimal MRP { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal TaxPercent { get; set; }
        public decimal NetAmount { get; set; }
    }
}
