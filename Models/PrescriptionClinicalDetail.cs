using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicApp.Models
{
    [Table("prescription_clinical_detail")]
    public class PrescriptionClinicalDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClinicalDetailId { get; set; }

        public int PrescriptionId { get; set; }

        [ForeignKey("PrescriptionId")]
        public Prescription Prescription { get; set; }

        [StringLength(100)]
        public string DetailType { get; set; }

        public string Description { get; set; }

        [StringLength(100)]
        public string Value { get; set; }

        [StringLength(50)]
        public string Unit { get; set; }

        public string Notes { get; set; }
    }
}
