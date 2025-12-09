using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicApp.Models
{
    [Table("SupplierMaster")]
    public class SupplierMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SupplierId { get; set; }

        [Required]
        [StringLength(255)]
        public string SupplierName { get; set; }

        [StringLength(255)]
        public string ContactPerson { get; set; }

        [StringLength(20)]
        public string MobileNo { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        public string Address { get; set; }

        [StringLength(50)]
        public string GSTNo { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation property
        public ICollection<ProductSupplier> ProductSuppliers { get; set; }
    }
}
