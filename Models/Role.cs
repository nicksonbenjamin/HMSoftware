// Models/Role.cs
using System;
using System.ComponentModel.DataAnnotations;

public class Role
{
    [Key]
    public Guid RoleId { get; set; }
    public string RoleName { get; set; }
}
