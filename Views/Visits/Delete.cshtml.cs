using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySql.Data.MySqlClient;
using ClinicApp.Data;

namespace ClinicApp.Visits
{
    public class DeleteModel : PageModel
    {
        private readonly ClinicAppMySqlDbContext _db;
        public Visit Visit { get; set; }

        public DeleteModel(ClinicAppMySqlDbContext db)
        {
            _db = db;
        }

        public void OnGet(string id)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand(@"
            SELECT v.*, p.Name AS PatientName, s.Name AS StaffName
            FROM Visits v
            JOIN Patients p ON v.PatientID = p.PatientID
            LEFT JOIN AttendingStaff s ON v.AttendingStaffID = s.StaffID
            WHERE v.VisitID=@Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                Visit = new Visit
                {
                    VisitID = reader.GetGuid("VisitID"),
                    PatientName = reader.GetString("PatientName"),
                    StaffName = reader.IsDBNull(reader.GetOrdinal("StaffName")) ? "" : reader.GetString("StaffName"),
                    VisitDate = reader.GetDateTime("VisitDate"),
                    Notes = reader.GetString("Notes")
                };
            }
        }

        public IActionResult OnPost(string id)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand("DELETE FROM Visits WHERE VisitID=@Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();
            return RedirectToPage("Index");
        }
    }
}