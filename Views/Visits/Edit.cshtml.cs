using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Mvc.Rendering;
using ClinicApp.Data;

namespace ClinicApp.Visits
{
    public class EditModel : PageModel
    {
        private readonly ClinicAppMySqlDbContext _db;

        [BindProperty]
        public Visit Visit { get; set; }
        public List<SelectListItem> Patients { get; set; }
        public List<SelectListItem> Staff { get; set; }

        public EditModel(ClinicAppMySqlDbContext db)
        {
            _db = db;
        }

        public void OnGet(string id)
        {
            Patients = new List<SelectListItem>();
            Staff = new List<SelectListItem>();

            using var conn = _db.GetConnection();
            conn.Open();

            var patientCmd = new MySqlCommand("SELECT PatientID, Name FROM Patients", conn);
            using var patientReader = patientCmd.ExecuteReader();
            while (patientReader.Read())
            {
                Patients.Add(new SelectListItem
                {
                    Value = patientReader.GetString("PatientID"),
                    Text = patientReader.GetString("Name")
                });
            }
            patientReader.Close();

            var staffCmd = new MySqlCommand("SELECT StaffID, Name FROM AttendingStaff", conn);
            using var staffReader = staffCmd.ExecuteReader();
            while (staffReader.Read())
            {
                Staff.Add(new SelectListItem
                {
                    Value = staffReader.GetInt32("StaffID").ToString(),
                    Text = staffReader.GetString("Name")
                });
            }
            staffReader.Close();

            var cmd = new MySqlCommand("SELECT * FROM Visits WHERE VisitID=@Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                Visit = new Visit
                {
                    VisitID = reader.GetGuid("VisitID"),
                    PatientID = reader.GetGuid("PatientID"),
                    VisitDate = reader.GetDateTime("VisitDate"),
                    Notes = reader.GetString("Notes"),
                    AttendingStaffID = reader.GetInt32("AttendingStaffID")
                };
            }
        }

        public IActionResult OnPost()
        {
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand(@"UPDATE Visits SET 
            PatientID=@PatientID, VisitDate=@VisitDate, Notes=@Notes, AttendingStaffID=@StaffID 
            WHERE VisitID=@VisitID", conn);
            cmd.Parameters.AddWithValue("@VisitID", Visit.VisitID);
            cmd.Parameters.AddWithValue("@PatientID", Visit.PatientID);
            cmd.Parameters.AddWithValue("@VisitDate", Visit.VisitDate);
            cmd.Parameters.AddWithValue("@Notes", Visit.Notes);
            cmd.Parameters.AddWithValue("@StaffID", Visit.AttendingStaffID);
            cmd.ExecuteNonQuery();
            return RedirectToPage("Index");
        }
    }
}