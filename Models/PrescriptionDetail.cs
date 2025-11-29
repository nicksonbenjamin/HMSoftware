using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicApp.Models
{
    [Table("prescription_details")]
    public class PrescriptionDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PrescriptionDetailId { get; set; }

        public int PrescriptionId { get; set; }

        [ForeignKey("PrescriptionId")]
        public DrPrescription Prescription { get; set; }

        [Required, StringLength(255)]
        public string Medicine { get; set; }

        [StringLength(50)]
        public string Dose { get; set; }

        [StringLength(100)]
        public string Instructions { get; set; }

        public int? Days { get; set; }
        public int? Quantity { get; set; }

        public string Note { get; set; }
    }
}
