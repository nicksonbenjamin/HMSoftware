using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using ClinicApp.Data;

namespace Roles
{
    public class EditModel : PageModel
    {
        private readonly ClinicAppMySqlDbContext _db;
        [BindProperty] public Role Role { get; set; }

        public EditModel(ClinicAppMySqlDbContext db)
        {
            _db = db;
        }

        public void OnGet(string id)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand("SELECT * FROM Roles WHERE RoleId=@Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                Role = new Role
                {
                    RoleId = reader.GetGuid("RoleId"),
                    RoleName = reader.GetString("RoleName")
                };
            }
        }

        public IActionResult OnPost()
        {
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand("UPDATE Roles SET RoleName=@Name WHERE RoleId=@Id", conn);
            cmd.Parameters.AddWithValue("@Id", Role.RoleId);
            cmd.Parameters.AddWithValue("@Name", Role.RoleName);
            cmd.ExecuteNonQuery();
            return RedirectToPage("Index");
        }
    }
}