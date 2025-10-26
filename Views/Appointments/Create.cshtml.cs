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
    public class CreateModel : PageModel
    {
        private readonly ClinicAppMySqlDbContext _db;
        [BindProperty] public Appointment Appointment { get; set; }
        public List<SelectListItem> Patients { get; set; }

        public CreateModel(ClinicAppMySqlDbContext db)
        {
            _db = db;
        }

        public void OnGet()
        {
            Patients = new List<SelectListItem>();
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand("SELECT PatientID, Name FROM Patients", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Patients.Add(new SelectListItem
                {
                    Value = reader.GetString("PatientID"),
                    Text = reader.GetString("Name")
                });
            }
        }

        public IActionResult OnPost()
        {
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand(@"INSERT INTO Appointments 
            (PatientID, AppointmentDate, Reason, Status) 
            VALUES (@PatientID, @Date, @Reason, @Status)", conn);
            cmd.Parameters.AddWithValue("@PatientID", Appointment.PatientID);
            cmd.Parameters.AddWithValue("@Date", Appointment.AppointmentDate);
            cmd.Parameters.AddWithValue("@Reason", Appointment.Reason);
            cmd.Parameters.AddWithValue("@Status", Appointment.Status ?? "Scheduled");
            cmd.ExecuteNonQuery();
            return RedirectToPage("Index");
        }
    }
}