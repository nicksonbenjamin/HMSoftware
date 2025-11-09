using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Mvc.Rendering;
using ClinicApp.Data;

namespace ClinicApp.ApplicationUsers
{
    public class CreateModel : PageModel
    {
        private readonly ClinicAppMySqlDbContext _db;
        [BindProperty] 
        public ApplicationUser User { get; set; }
        public List<SelectListItem> Roles { get; set; }

        public CreateModel(ClinicAppMySqlDbContext db)
        {
            _db = db;
        }

        public void OnGet()
        {
            Roles = new List<SelectListItem>();
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand("SELECT RoleId, RoleName FROM Roles", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Roles.Add(new SelectListItem
                {
                    Value = reader.GetGuid("RoleId").ToString(),
                    Text = reader.GetString("RoleName")
                });
            }
        }

        public IActionResult OnPost()
        {
            User.UserId = Guid.NewGuid();
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand(@"INSERT INTO ApplicationUser 
            (UserId, LoginNamee, RoleID, LoginPassword, FirstName, LastName, MiddleName) 
            VALUES (@UserId, @LoginNamee, @RoleID, @LoginPassword, @FirstName, @LastName, @MiddleName)", conn);
            cmd.Parameters.AddWithValue("@UserId", User.UserId);
            cmd.Parameters.AddWithValue("@LoginNamee", User.LoginNamee);
            cmd.Parameters.AddWithValue("@RoleID", User.RoleID);
            cmd.Parameters.AddWithValue("@LoginPassword", User.LoginPassword);
            cmd.Parameters.AddWithValue("@FirstName", User.FirstName);
            cmd.Parameters.AddWithValue("@LastName", User.LastName);
            cmd.Parameters.AddWithValue("@MiddleName", User.MiddleName);
            cmd.ExecuteNonQuery();
            return RedirectToPage("Index");
        }
    }
}