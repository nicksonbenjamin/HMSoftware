using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicApp.Models
{
   public class PrescriptionDetail
    {
        [Key]
        public int DetailId { get; set; }

        [ForeignKey("Prescription")]
        public int PrescriptionDetailId  { get; set; }

        [Required]
        public string Description { get; set; }

        [DataType(DataType.Currency)]
        public decimal? Amount { get; set; }

        public Prescription Prescription { get; set; }
    }
}