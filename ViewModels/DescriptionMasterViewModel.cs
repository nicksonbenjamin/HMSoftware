using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClinicApp.ViewModels
{
    public class DescriptionMasterViewModel
    {
        // DescriptionMaster fields
        public int DescriptionId { get; set; }

        [StringLength(50)]
        public string DescriptionCode { get; set; }

        [Required]
        [StringLength(255)]
        public string DescriptionName { get; set; }

        [StringLength(100)]
        public string Group { get; set; }

        [StringLength(100)]
        public string Section { get; set; }

        public bool Applicable_All { get; set; }
        public bool Applicable_LAB { get; set; }
        public bool Applicable_OP { get; set; }
        public bool Consultation { get; set; }

        public decimal NormalCharges { get; set; }
        public decimal EmergencyCharges { get; set; }
        public decimal DrCharges { get; set; }

        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // List of related test values
        public List<DescriptionMasterTestValueViewModel> TestValues { get; set; } = new List<DescriptionMasterTestValueViewModel>();
    }

    public class DescriptionMasterTestValueViewModel
    {
        public int TestValueID { get; set; }

        public int DescriptionId { get; set; }  // FK to DescriptionMaster

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
    }
}
