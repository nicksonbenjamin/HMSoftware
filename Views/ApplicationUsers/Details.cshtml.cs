using ClinicApp.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace ClinicApp.ApplicationUsers
{
    public class DetailsModel : PageModel
    {
        private readonly ClinicAppMySqlDbContext _db;
        public ApplicationUser User { get; set; }

        public DetailsModel(ClinicAppMySqlDbContext db)
        {
            _db = db;
        }

        public void OnGet(string id)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand(@"
            SELECT u.*, r.Name AS RoleName
            FROM ApplicationUser u
            LEFT JOIN Roles r ON u.RoleID = r.RoleId
            WHERE u.UserId=@Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                User = new ApplicationUser
                {
                    UserId = reader.GetGuid("UserId"),
                    LoginNamee = reader.GetString("LoginNamee"),
                    RoleID = reader.GetGuid("RoleID"),
                    LoginPassword = reader.GetString("LoginPassword"),
                    FirstName = reader.GetString("FirstName"),
                    LastName = reader.GetString("LastName"),
                    MiddleName = reader.GetString("MiddleName"),
                    RoleName = reader.IsDBNull(reader.GetOrdinal("RoleName")) ? "" : reader.GetString("RoleName")
                };
            }
        }
    }
}