using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicApp.ViewModels
{
    [Table("common_master")]
    public class CommonMasterViewModel
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("master_type")]
        [StringLength(50)]
        public string MasterType { get; set; }

        [Required]
        [Column("master_name")]
        [StringLength(255)]
        public string MasterName { get; set; }
    }
}
