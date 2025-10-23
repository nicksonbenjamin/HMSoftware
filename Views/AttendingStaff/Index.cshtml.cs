using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using ClinicApp.Data;

namespace ClinicApp.AttendingStaff
{
    public class IndexModel : PageModel
    {
        private readonly ClinicAppMySqlDbContext _db;
        public List<ClinicApp.Models.AttendingStaff> StaffList { get; set; }

        public IndexModel(ClinicAppMySqlDbContext db)
        {
            _db = db;
        }

        public void OnGet()
        {
            StaffList = new List<ClinicApp.Models.AttendingStaff>();
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand("SELECT * FROM AttendingStaff", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                StaffList.Add(new ClinicApp.Models.AttendingStaff
                {
                    StaffID = reader.GetInt32("StaffID"),
                    FirstName = reader.GetString("FirstName"),
                    LastName = reader.GetString("LastName"),
                    Role = reader.GetString("Role"),
                    Email = reader.GetString("Email"),
                    PhoneNumber = reader.GetString("PhoneNumber"),
                    CreatedAt = reader.GetDateTime("CreatedAt")
                });
            }
        }
    }
}