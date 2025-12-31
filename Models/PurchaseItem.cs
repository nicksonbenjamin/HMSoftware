using System;

namespace ClinicApp.Models
{
    public class PurchaseItem
    {
        public int Id { get; set; }
        public int PurchaseId { get; set; }
        public int ProductId { get; set; }
        public string BatchNo { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public decimal Qty { get; set; }
        public decimal FreeQty { get; set; }
        public decimal Rate { get; set; }
        public decimal MRP { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal TaxPercent { get; set; }
        public decimal NetAmount { get; set; }

        // Navigation
        public PurchaseMaster Purchase { get; set; }
        public ProductMaster Product { get; set; }
    }
}
