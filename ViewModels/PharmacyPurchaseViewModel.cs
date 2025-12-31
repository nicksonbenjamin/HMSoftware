using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClinicApp.ViewModels
{
    public class PharmacyPurchaseViewModel
    {
        public int PurchaseId { get; set; }

        [Required]
        public int ProductCode { get; set; }

        [Required]
        public int SupplierId { get; set; }

        [Required]
        [StringLength(50)]
        public string BatchNo { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Quantity { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal PurchaseRate { get; set; }

        [Range(0, double.MaxValue)]
        public decimal MRP { get; set; }

        [Range(0, 100)]
        public decimal TaxPercent { get; set; }

        [Range(0, double.MaxValue)]
        public decimal SaleRate { get; set; }

        [StringLength(50)]
        public string Rack { get; set; }

        [StringLength(50)]
        public string Bin { get; set; }

        public bool IsActive { get; set; } = true;

        // Optional: show names in dropdown
        public string ProductName { get; set; }
        public string SupplierName { get; set; }

        // For dropdown lists
        public List<ProductMasterDropdown> ProductList { get; set; } = new List<ProductMasterDropdown>();
        public List<SupplierDropdown> SupplierList { get; set; } = new List<SupplierDropdown>();
    }

    public class ProductMasterDropdown
    {
        public int ProductCode { get; set; }
        public string ProductName { get; set; }
    }

    public class SupplierDropdown
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
    }
}
