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
    public class IndexModel : PageModel
    {
        private readonly ClinicAppMySqlDbContext _db;
        public List<Appointment> Appointments { get; set; }

        public IndexModel(ClinicAppMySqlDbContext db)
        {
            _db = db;
        }

        public void OnGet()
        {
            Appointments = new List<Appointment>();
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand(@"
            SELECT a.*, p.Name AS PatientName
            FROM Appointments a
            JOIN Patients p ON a.PatientID = p.PatientID", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Appointments.Add(new Appointment
                {
                    AppointmentID = reader.GetInt32("AppointmentID"),
                    PatientID = reader.GetString("PatientID"),
                    AppointmentDate = reader.GetDateTime("AppointmentDate"),
                    Reason = reader.GetString("Reason"),
                    Status = reader.GetString("Status"),
                    CreatedAt = reader.GetDateTime("CreatedAt"),
                    PatientName = reader.GetString("PatientName")
                });
            }
        }
    }
}