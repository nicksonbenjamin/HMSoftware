using System;
using System.Collections.Generic;

namespace ClinicApp.Models
{
    public class PurchaseMaster
    {
        public int Id { get; set; }
        public string PurchaseNo { get; set; }
        public DateTime PurchaseDate { get; set; }

        // Supplier reference
        public int SupplierId { get; set; }
        public LedgerMaster Supplier { get; set; } // Navigation property

        public string SupplierInvoiceNo { get; set; }
        public DateTime? SupplierInvoiceDate { get; set; }

        public decimal TotalQty { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalTax { get; set; }
        public decimal RoundOff { get; set; }
        public decimal NetAmount { get; set; }
        public string Remarks { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Child items
        public List<PurchaseItem> PurchaseItems { get; set; } = new List<PurchaseItem>();
    }
}
