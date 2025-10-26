// Models/ApplicationUser.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ApplicationUser
{
    public Guid UserId { get; set; }
    public string LoginNamee { get; set; }
    public Guid RoleID { get; set; }
    public string LoginPassword { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }

    // Optional: Display Role Name
    public string RoleName { get; set; }
}