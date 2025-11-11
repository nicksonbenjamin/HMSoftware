using System.ComponentModel.DataAnnotations;

public class CommonMaster
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string MasterType { get; set; }

    [Required]
    [StringLength(255)]
    public string MasterName { get; set; }
}
