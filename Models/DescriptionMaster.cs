using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicApp.Models
{
    [Table("DescriptionMaster")]
    public class DescriptionMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DescriptionId { get; set; }  // Primary Key, auto-increment

        [StringLength(50)]
        public string DescriptionCode { get; set; }

        [Required]
        [StringLength(255)]
        public string DescriptionName { get; set; }

        [StringLength(100)]
        public string Group { get; set; }

        [StringLength(100)]
        public string Section { get; set; }

        public bool Applicable_All { get; set; } = false;
        public bool Applicable_LAB { get; set; } = false;
        public bool Applicable_OP { get; set; } = false;
        public bool Consultation { get; set; } = false;

        [Column(TypeName = "decimal(10,2)")]
        public decimal NormalCharges { get; set; } = 0.00m;

        [Column(TypeName = "decimal(10,2)")]
        public decimal EmergencyCharges { get; set; } = 0.00m;

        [Column(TypeName = "decimal(10,2)")]
        public decimal DrCharges { get; set; } = 0.00m;

        public string Remarks { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation property — one Description can have many Test Values
        public ICollection<DescriptionMasterTestValue> TestValues { get; set; }
    }
}
