using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicApp.Models
{
    public class DescriptionMasterTestValue
    {
        [Key]
        public int TestValueID { get; set; }  // Primary Key

        // Foreign key linking to DescriptionsMaster
        [ForeignKey("DescriptionsMaster")]
        public string DescriptionCode { get; set; }  // Use string to match DescriptionsMaster PK

        [StringLength(100)]
        public string Specimen { get; set; }

        [StringLength(255)]
        public string TestName { get; set; }

        [StringLength(100)]
        public string Male { get; set; }

        [StringLength(100)]
        public string Female { get; set; }

        [StringLength(100)]
        public string Children { get; set; }

        [StringLength(100)]
        public string General { get; set; }

        [StringLength(50)]
        public string Unit { get; set; }

        [StringLength(100)]
        public string Method { get; set; }

        [StringLength(255)]
        public string HtmlStyle { get; set; }

        [StringLength(100)]
        public string Instrument { get; set; }

        public int SortOrder { get; set; }

        // Navigation property
        public DescriptionMaster DescriptionMaster { get; set; }
    }
}
