using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Mvc.Rendering;
using ClinicApp.Data;

namespace ClinicApp.ApplicationUsers
{
    public class IndexModel : PageModel
    {
        private readonly ClinicAppMySqlDbContext _db;
        public List<ApplicationUser> Users { get; set; }

        public IndexModel(ClinicAppMySqlDbContext db)
        {
            _db = db;
        }

        public void OnGet()
        {
            Users = new List<ApplicationUser>();
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand(@"SELECT u.*, r.Name AS RoleName
            FROM ApplicationUser u
            LEFT JOIN Roles r ON u.RoleID = r.RoleId", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Users.Add(new ApplicationUser
                {
                    UserId = reader.GetGuid("UserId"),
                    LoginNamee = reader.GetString("LoginNamee"),
                    RoleID = reader.GetGuid("RoleID"),
                    LoginPassword = reader.GetString("LoginPassword"),
                    FirstName = reader.GetString("FirstName"),
                    LastName = reader.GetString("LastName"),
                    MiddleName = reader.GetString("MiddleName"),
                    RoleName = reader.IsDBNull(reader.GetOrdinal("RoleName")) ? "" : reader.GetString("RoleName")
                });
            }
        }
    }
}