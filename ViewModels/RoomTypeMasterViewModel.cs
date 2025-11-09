using System.ComponentModel.DataAnnotations;
using ClinicApp.Models; // adjust namespace as per your project
namespace ClinicApp.ViewModels
{
    public class RoomTypeMasterViewModel
    {
        public int TypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string TypeName { get; set; }
    }
}
