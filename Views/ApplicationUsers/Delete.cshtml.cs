using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using ClinicApp.Data;

namespace ClinicApp.ApplicationUsers
{
    public class DeleteModel : PageModel
    {
        private readonly ClinicAppMySqlDbContext _db;
        public ApplicationUser User { get; set; }

        public DeleteModel(ClinicAppMySqlDbContext db)
        {
            _db = db;
        }

        public void OnGet(string id)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand("SELECT * FROM ApplicationUser WHERE UserId=@Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                User = new ApplicationUser
                {
                    UserId = reader.GetGuid("UserId"),
                    LoginNamee = reader.GetString("LoginNamee"),
                    FirstName = reader.GetString("FirstName"),
                    MiddleName = reader.GetString("MiddleName"),
                    LastName = reader.GetString("LastName")
                };
            }
        }

        public IActionResult OnPost(string id)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand("DELETE FROM ApplicationUser WHERE UserId=@UserId", conn);
            cmd.Parameters.AddWithValue("@UserId", id);
            cmd.ExecuteNonQuery();
            return RedirectToPage("Index");
        }
    }
}