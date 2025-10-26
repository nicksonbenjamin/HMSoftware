using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using ClinicApp.Data;

namespace ClinicApp.AttendingStaff
{
    public class EditModel : PageModel
    {
        private readonly ClinicAppMySqlDbContext _db;
        [BindProperty] 
        public ClinicApp.Models.AttendingStaff Staff { get; set; }

        public EditModel(ClinicAppMySqlDbContext db)
        {
            _db = db;
        }

        public void OnGet(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand("SELECT * FROM AttendingStaff WHERE StaffID=@Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                Staff = new ClinicApp.Models.AttendingStaff
                {
                    StaffID = reader.GetInt32("StaffID"),
                    FirstName = reader.GetString("FirstName"),
                    LastName = reader.GetString("LastName"),
                    Role = reader.GetString("Role"),
                    Email = reader.GetString("Email"),
                    PhoneNumber = reader.GetString("PhoneNumber"),
                    CreatedAt = reader.GetDateTime("CreatedAt")
                };
            }
        }

        public IActionResult OnPost()
        {
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand(@"UPDATE AttendingStaff SET 
            FirstName=@FirstName, LastName=@LastName, Role=@Role, 
            Email=@Email, PhoneNumber=@Phone WHERE StaffID=@Id", conn);
            cmd.Parameters.AddWithValue("@Id", Staff.StaffID);
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