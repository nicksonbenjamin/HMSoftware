using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicApp.Models
{
    [Table("ProductSupplier")]
   public class ProductSupplier
{
    public int ProductSupplierId { get; set; } // PK
    public int ProductCode { get; set; }       // FK to ProductMaster
    public int SupplierId { get; set; }        // FK to SupplierMaster

    // Navigation
    public ProductMaster Product { get; set; }
    public SupplierMaster Supplier { get; set; }
}

}
