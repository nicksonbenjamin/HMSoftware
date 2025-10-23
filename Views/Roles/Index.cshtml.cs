using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using ClinicApp.Data;

namespace Roles
{
    public class IndexModel : PageModel
    {
        private readonly ClinicAppMySqlDbContext _db;
        public List<Role> Roles { get; set; }

        public IndexModel(ClinicAppMySqlDbContext db)
        {
            _db = db;
        }

        public void OnGet()
        {
            Roles = new List<Role>();
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand("SELECT * FROM Roles", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Roles.Add(new Role
                {
                    RoleId = reader.GetGuid("RoleId"),
                    RoleName = reader.GetString("RoleName")
                });
            }
        }
    }
}