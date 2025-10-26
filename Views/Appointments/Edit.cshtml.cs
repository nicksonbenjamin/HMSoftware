using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySql.Data.MySqlClient;
using ClinicApp.Data;

namespace ClinicApp.Appointments
{
    public class EditModel : PageModel
    {
        private readonly ClinicAppMySqlDbContext _db;
        [BindProperty] public Appointment Appointment { get; set; }
        public List<SelectListItem> Patients { get; set; }

        public EditModel(ClinicAppMySqlDbContext db)
        {
            _db = db;
        }

        public void OnGet(int id)
        {
            Patients = new List<SelectListItem>();
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

            var cmd = new MySqlCommand("SELECT * FROM Appointments WHERE AppointmentID=@Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                Appointment = new Appointment
                {
                    AppointmentID = reader.GetInt32("AppointmentID"),
                    PatientID = reader.GetString("PatientID"),
                    AppointmentDate = reader.GetDateTime("AppointmentDate"),
                    Reason = reader.GetString("Reason"),
                    Status = reader.GetString("Status"),
                    CreatedAt = reader.GetDateTime("CreatedAt")
                };
            }
        }

        public IActionResult OnPost()
        {
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand(@"UPDATE Appointments SET 
            PatientID=@PatientID, AppointmentDate=@Date, Reason=@Reason, Status=@Status 
            WHERE AppointmentID=@Id", conn);
            cmd.Parameters.AddWithValue("@Id", Appointment.AppointmentID);
            cmd.Parameters.AddWithValue("@PatientID", Appointment.PatientID);
            cmd.Parameters.AddWithValue("@Date", Appointment.AppointmentDate);
            cmd.Parameters.AddWithValue("@Reason", Appointment.Reason);
            cmd.Parameters.AddWithValue("@Status", Appointment.Status);
            cmd.ExecuteNonQuery();
            return RedirectToPage("Index");
        }
    }
}