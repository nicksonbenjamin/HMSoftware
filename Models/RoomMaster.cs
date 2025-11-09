using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicApp.Models
{
    [Table("room_master")]
    public class RoomMaster
    {
        [Key]
        [Column("room_id")]
        public int RoomId { get; set; }

        [Required]
        [Column("room_no")]
        [StringLength(10)]
        public string RoomNo { get; set; }

        [Column("floor_no")]
        [StringLength(10)]
        public string FloorNo { get; set; }

        [ForeignKey("RoomType")]
        [Column("type_id")]
        public int? TypeId { get; set; }

        public string RoomTypeName { get; set; }

        public RoomTypeMaster RoomType { get; set; }

        [Column("no_of_beds")]
        public int? NoOfBeds { get; set; }

        [Column("rent_per_day")]
        public decimal? RentPerDay { get; set; }

        [Column("rent_per_hour")]
        public decimal? RentPerHour { get; set; }

        [Column("nursing_charge_per_day")]
        public decimal? NursingChargePerDay { get; set; }

        [Column("remarks")]
        [StringLength(255)]
        public string Remarks { get; set; }

        [Column("charge_description")]
        [StringLength(100)]
        public string ChargeDescription { get; set; }

        [Column("amount_per_day")]
        public decimal? AmountPerDay { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;
    }
}
