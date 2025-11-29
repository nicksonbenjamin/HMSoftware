using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicApp.Models
{
    [Table("DrPrescriptionClinical")]
    public class DrPrescriptionClinical
    {
        [Key]
        public int DrPrescriptionClinicalId { get; set; }
        public int DrPrescriptionId { get; set; }
        public string ClinicalNote { get; set; }
        public string Result { get; set; }
    }
}
