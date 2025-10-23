using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using ClinicApp.Models;
using ClinicApp.Data;

namespace ClinicApp.AttendingStaff
{
    public class CreateModel : PageModel
    {
        private readonly ClinicAppMySqlDbContext _db;

        [BindProperty]
        public ClinicApp.Models.AttendingStaff Staff { get; set; }

        public CreateModel(ClinicAppMySqlDbContext db)
        {
            _db = db;
        }

        public void OnGet() { }

        public IActionResult OnPost()
        {
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand(@"INSERT INTO AttendingStaff 
            (FirstName, LastName, Role, Email, PhoneNumber) 
            VALUES (@FirstName, @LastName, @Role, @Email, @Phone)", conn);
            cmd.Parameters.AddWithValue("@FirstName", Staff.FirstName);
            cmd.Parameters.AddWithValue("@LastName", Staff.LastName);
            cmd.Parameters.AddWithValue("@Role", Staff.Role);
            cmd.Parameters.AddWithValue("@Email", Staff.Email);
            cmd.Parameters.AddWithValue("@Phone", Staff.PhoneNumber);
            cmd.ExecuteNonQuery();
            return RedirectToPage("Index");
        }
    }
}