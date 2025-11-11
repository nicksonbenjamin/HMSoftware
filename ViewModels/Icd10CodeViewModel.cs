using System;
using System.ComponentModel.DataAnnotations;

namespace ClinicApp.ViewModels
{
    public class Icd10CodeViewModel
    {
        // ICDCodeMaster fields
        public int ICDCodeMasterId { get; set; }

        [StringLength(10)]
        public string ICDCode { get; set; }

        [Required]
        [StringLength(255)]
        public string DiagnosisCondition { get; set; }

        [StringLength(255)]
        public string DescriptionUsage { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
