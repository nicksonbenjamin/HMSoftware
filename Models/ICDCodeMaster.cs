using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicApp.Models
{
    [Table("icdcode_master")]
    public class ICDCodeMaster
    {
        [Key]
        [Column("icdcodemasterId")]
        public int ICDCodeMasterId { get; set; }

        [Column("icd_code")]
        [StringLength(10)]
        public string ICDCode { get; set; }

        [Column("diagnosis_condition")]
        [Required]
        [StringLength(255)]
        public string DiagnosisCondition { get; set; }

        [Column("description_usage")]
        [StringLength(255)]
        public string DescriptionUsage { get; set; }

        [Column("IsActive")]
        public bool IsActive { get; set; } = true;

        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("UpdatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
