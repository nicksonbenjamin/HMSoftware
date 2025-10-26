using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClinicApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using ClinicApp.Data;

namespace Roles
{
    public class CreateModel : PageModel
    {
        private readonly ClinicAppMySqlDbContext _db;
        [BindProperty] public Role Role { get; set; }

        public CreateModel(ClinicAppMySqlDbContext db)
        {
            _db = db;
        }

        public void OnGet() { }

        public IActionResult OnPost()
        {
            Role.RoleId = Guid.NewGuid();
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand("INSERT INTO Roles (RoleId, RoleName) VALUES (@Id, @Name)", conn);
            cmd.Parameters.AddWithValue("@Id", Role.RoleId);
            cmd.Parameters.AddWithValue("@Name", Role.RoleName);
            cmd.ExecuteNonQuery();
            return RedirectToPage("Index");
        }
    }
}