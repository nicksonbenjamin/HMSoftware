// Models/Patient.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClinicApp.ViewModels
{
    public class ProductMasterViewModel
    {
        public int ProductCode { get; set; }

        [Required]
        [StringLength(100)]
        public string ProductName { get; set; }

        public string GenericName { get; set; }

        public string ProductGroup_List { get; set; }
        public string ProductGroup_Txt { get; set; }

        public string PurchaseUnit_List { get; set; }
        public string PurchaseUnit_Txt { get; set; }

        public int Packing { get; set; }

        public string StockUnit_List { get; set; }
        public string StockUnit_Txt { get; set; }

        public string Rack_List { get; set; }
        public string Rack_Txt { get; set; }

        public string Bin_List { get; set; }
        public string Bin_Txt { get; set; }

        public string ProductType_List { get; set; }
        public string ProductType_Txt { get; set; }

        public string Manufacturer_List { get; set; }
        public string Manufacturer_Txt { get; set; }

        public string Remarks { get; set; }

        [Range(0, double.MaxValue)]
        public decimal PurchaseRate { get; set; }

        [Range(0, 100)]
        public decimal PurchaseDiscount { get; set; }

        [Range(0, double.MaxValue)]
        public decimal MRP { get; set; }

        [Range(0, double.MaxValue)]
        public decimal SaleRate { get; set; }

        [Range(0, 100)]
        public decimal SaleDiscount { get; set; }

        [Range(0, 100)]
        public decimal TaxPercent { get; set; }

        public string HSN { get; set; }

        [Range(0, double.MaxValue)]
        public decimal MinStock { get; set; }

        [Range(0, double.MaxValue)]
        public decimal MaxStock { get; set; }

        [Range(0, double.MaxValue)]
        public decimal MinOrder { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string ErrorMessage { get; set; }
    }
}