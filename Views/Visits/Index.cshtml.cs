using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using ClinicApp.Data;

namespace ClinicApp.Visits
{
    public class IndexModel : PageModel
    {
        private readonly ClinicAppMySqlDbContext _db;
        public List<Visit> Visits { get; set; }

        public IndexModel(ClinicAppMySqlDbContext db)
        {
            _db = db;
        }

        public void OnGet()
        {
            Visits = new List<Visit>();
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand(@"
            SELECT v.*, p.Name AS PatientName, s.Name AS StaffName
            FROM Visits v
            JOIN Patients p ON v.PatientID = p.PatientID
            LEFT JOIN AttendingStaff s ON v.AttendingStaffID = s.StaffID", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Visits.Add(new Visit
                {
                    VisitID = reader.GetGuid("VisitID"),
                    PatientID = reader.GetGuid("PatientID"),
                    VisitDate = reader.GetDateTime("VisitDate"),
                    Notes = reader.GetString("Notes"),
                    AttendingStaffID = reader.GetInt32("AttendingStaffID"),
                    PatientName = reader.GetString("PatientName"),
                    StaffName = reader.IsDBNull(reader.GetOrdinal("StaffName")) ? "" : reader.GetString("StaffName")
                });
            }
        }
    }
}