using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using ClinicApp.Data;

namespace Roles
{
    public class DetailsModel : PageModel
    {
        private readonly ClinicAppMySqlDbContext _db;
        public Role Role { get; set; }

        public DetailsModel(ClinicAppMySqlDbContext db)
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
    }
}