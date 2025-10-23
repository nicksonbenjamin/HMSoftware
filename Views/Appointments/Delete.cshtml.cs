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
    public class DeleteModel : PageModel
    {
        private readonly ClinicAppMySqlDbContext _db;
        public Appointment Appointment { get; set; }

        public DeleteModel(ClinicAppMySqlDbContext db)
        {
            _db = db;
        }

        public void OnGet(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand(@"
            SELECT a.*, p.Name AS PatientName
            FROM Appointments a
            JOIN Patients p ON a.PatientID = p.PatientID
            WHERE a.AppointmentID=@Id", conn);
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
                    CreatedAt = reader.GetDateTime("CreatedAt"),
                    PatientName = reader.GetString("PatientName")
                };
            }
        }

        public IActionResult OnPost(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand("DELETE FROM Appointments WHERE AppointmentID=@Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();
            return RedirectToPage("Index");
        }
    }
}