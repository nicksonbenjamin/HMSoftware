using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicApp.Models
{
    [Table("room_type_master")]
    public class RoomTypeMaster
    {
        [Key]
        [Column("type_id")]
        public int TypeId { get; set; }

        [Required]
        [Column("type_name")]
        [StringLength(50)]
        public string TypeName { get; set; }

        // Optional: Navigation property (one-to-many)
        public ICollection<RoomMaster> RoomMasters { get; set; }
    }
}
