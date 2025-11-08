using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using ClinicApp.ViewModels;
using ClinicApp.Data;
using ClinicApp.Models;

namespace ClinicApp.Controllers
{
    public class PrescriptionsController : Controller
    {
        private readonly ILogger<PrescriptionsController> _logger;
        private readonly ClinicAppMySqlDbContext _db;
        private readonly AutoMapper.IMapper _mapper;

        public PrescriptionsController(ILogger<PrescriptionsController> logger, ClinicAppMySqlDbContext db, AutoMapper.IMapper mapper)
        {
            _logger = logger;
            _db = db;
            _mapper = mapper;
        }

        // GET: Prescriptions
        public IActionResult Index()
        {
            var prescriptions = new List<Prescription>();
            using var conn = _db.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand("SELECT * FROM Prescriptions", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                prescriptions.Add(new Prescription
                {
                    PrescriptionId = reader.IsDBNull(reader.GetOrdinal("PrescriptionId")) ? 0 : reader.GetInt32("PrescriptionId"),
                    EntryType = reader.IsDBNull(reader.GetOrdinal("EntryType")) ? null : reader["EntryType"]?.ToString(),
                    EntryNo = reader.IsDBNull(reader.GetOrdinal("EntryNo")) ? null : reader["EntryNo"]?.ToString(),
                    EntryDate = reader.IsDBNull(reader.GetOrdinal("EntryDate")) ? default : reader.GetDateTime("EntryDate"),
                    PatientName = reader.IsDBNull(reader.GetOrdinal("PatientName")) ? null : reader["PatientName"]?.ToString()
                });
            }

            return View(prescriptions);
        }

        // GET: Prescriptions/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();

            PrescriptionViewModel prescription = null;

            using var conn = _db.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand("SELECT * FROM Prescriptions WHERE PrescriptionId=@PrescriptionId", conn);
            cmd.Parameters.AddWithValue("@PrescriptionId", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                prescription = MapReaderToViewModel(reader);
            }

            if (prescription != null)
            {
                prescription.Details = GetPrescriptionDetails((int)id, conn);
            }

            return prescription == null ? NotFound() : View(prescription);
        }

        // GET: Prescriptions/Create
        public IActionResult Create()
        {
            var model = new PrescriptionViewModel
            {
                Details = new List<PrescriptionDetailViewModel>()
            };
            return View(model);
        }

        // POST: Prescriptions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PrescriptionViewModel prescription)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Prescriptions 
                (EntryType, EntryNo, EntryDate, UHID, PatientName, Address, Place, CellNo, Sex, Age, BloodGroup, 
                 HeightCm, WeightKg, BMI, Temperature, BP, SPO2, PulseRate, ConsultBy, ReferredBy, Disease, Remarks, 
                 TotalAmount, NextVisitAfter, NextVisitDate)
                VALUES
                (@EntryType, @EntryNo, @EntryDate, @UHID, @PatientName, @Address, @Place, @CellNo, @Sex, @Age, @BloodGroup, 
                 @HeightCm, @WeightKg, @BMI, @Temperature, @BP, @SPO2, @PulseRate, @ConsultBy, @ReferredBy, @Disease, @Remarks, 
                 @TotalAmount, @NextVisitAfter, @NextVisitDate);
                SELECT LAST_INSERT_ID();";

            AddMySqlParameters(cmd, prescription);
            int newPrescriptionId = Convert.ToInt32(cmd.ExecuteScalar());

            // Save details
            if (prescription.Details != null && prescription.Details.Count > 0)
            {
                foreach (var detail in prescription.Details)
                {
                    var detailCmd = conn.CreateCommand();
                    detailCmd.CommandText = @"
                        INSERT INTO PrescriptionDetail (PrescriptionId, Description, Amount)
                        VALUES (@PrescriptionId, @Description, @Amount)";
                    AddPrescriptionDetailParameters(detailCmd, detail, newPrescriptionId);
                    detailCmd.ExecuteNonQuery();
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Prescriptions/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();

            PrescriptionViewModel prescription = null;
            using var conn = _db.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand("SELECT * FROM Prescriptions WHERE PrescriptionId=@PrescriptionId", conn);
            cmd.Parameters.AddWithValue("@PrescriptionId", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                prescription = MapReaderToViewModel(reader);
            }

            if (prescription != null)
            {
                prescription.Details = GetPrescriptionDetails((int)id, conn);
            }

            return prescription == null ? NotFound() : View(prescription);
        }

        // POST: Prescriptions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, PrescriptionViewModel prescription)
        {
            if (id != prescription.PrescriptionId)
                return NotFound();

            using var conn = _db.GetConnection();
            conn.Open();

            // Update Prescription
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                UPDATE Prescriptions SET
                EntryType=@EntryType, EntryNo=@EntryNo, EntryDate=@EntryDate, UHID=@UHID, PatientName=@PatientName,
                Address=@Address, Place=@Place, CellNo=@CellNo, Sex=@Sex, Age=@Age, BloodGroup=@BloodGroup,
                HeightCm=@HeightCm, WeightKg=@WeightKg, BMI=@BMI, Temperature=@Temperature, BP=@BP, SPO2=@SPO2,
                PulseRate=@PulseRate, ConsultBy=@ConsultBy, ReferredBy=@ReferredBy, Disease=@Disease, Remarks=@Remarks,
                TotalAmount=@TotalAmount, NextVisitAfter=@NextVisitAfter, NextVisitDate=@NextVisitDate
                WHERE PrescriptionId=@PrescriptionId";

            cmd.Parameters.AddWithValue("@PrescriptionId", prescription.PrescriptionId);
            AddMySqlParameters(cmd, prescription);
            cmd.ExecuteNonQuery();

            // Delete old details
            var deleteCmd = conn.CreateCommand();
            deleteCmd.CommandText = "DELETE FROM PrescriptionDetail WHERE PrescriptionId=@PrescriptionId";
            deleteCmd.Parameters.AddWithValue("@PrescriptionId", prescription.PrescriptionId);
            deleteCmd.ExecuteNonQuery();

            // Insert new details
            if (prescription.Details != null && prescription.Details.Count > 0)
            {
                foreach (var detail in prescription.Details)
                {
                    var detailCmd = conn.CreateCommand();
                    detailCmd.CommandText = @"
                        INSERT INTO PrescriptionDetail (PrescriptionId, Description, Amount)
                        VALUES (@PrescriptionId, @Description, @Amount)";
                  if (prescription.PrescriptionId.HasValue)
{
    AddPrescriptionDetailParameters(detailCmd, detail, prescription.PrescriptionId.Value);
}
                    detailCmd.ExecuteNonQuery();
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Prescriptions/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            PrescriptionViewModel prescription = null;
            using var conn = _db.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand("SELECT PrescriptionId, EntryType, EntryNo, EntryDate, PatientName FROM Prescriptions WHERE PrescriptionId=@PrescriptionId", conn);
            cmd.Parameters.AddWithValue("@PrescriptionId", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                prescription = new PrescriptionViewModel
                {
                    PrescriptionId = reader.GetInt32("PrescriptionId"),
                    EntryType = reader.GetString("EntryType"),//]?.ToString(),
                    EntryNo = reader.GetString("EntryNo"),//["EntryNo"]?.ToString(),
                    EntryDate = reader.GetDateTime("EntryDate"),
                    PatientName = reader.GetString("PatientName")//["PatientName"]?.ToString()
                };
            }

            return prescription == null ? NotFound() : View(prescription);
        }

        // POST: Prescriptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand("DELETE FROM Prescriptions WHERE PrescriptionId=@PrescriptionId", conn);
            cmd.Parameters.AddWithValue("@PrescriptionId", id);
            cmd.ExecuteNonQuery();

            // Also delete related details
            var detailCmd = conn.CreateCommand();
            detailCmd.CommandText = "DELETE FROM PrescriptionDetail WHERE PrescriptionId=@PrescriptionId";
            detailCmd.Parameters.AddWithValue("@PrescriptionId", id);
            detailCmd.ExecuteNonQuery();

            return RedirectToAction(nameof(Index));
        }

        #region Helpers

        private static PrescriptionViewModel MapReaderToViewModel(MySqlDataReader reader)
        {
            return new PrescriptionViewModel
            {
                PrescriptionId = reader.IsDBNull(reader.GetOrdinal("PrescriptionId")) ? 0 : reader.GetInt32("PrescriptionId"),
                EntryType = reader.IsDBNull(reader.GetOrdinal("EntryType")) ? null : reader["EntryType"].ToString(),
                EntryNo = reader.IsDBNull(reader.GetOrdinal("EntryNo")) ? null : reader["EntryNo"].ToString(),
                EntryDate = reader.IsDBNull(reader.GetOrdinal("EntryDate")) ? (DateTime?)null : reader.GetDateTime("EntryDate"),
                UHID = reader.IsDBNull(reader.GetOrdinal("UHID")) ? null : reader["UHID"].ToString(),
                PatientName = reader.IsDBNull(reader.GetOrdinal("PatientName")) ? null : reader["PatientName"].ToString(),
                Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader["Address"].ToString(),
                Place = reader.IsDBNull(reader.GetOrdinal("Place")) ? null : reader["Place"].ToString(),
                CellNo = reader.IsDBNull(reader.GetOrdinal("CellNo")) ? null : reader["CellNo"].ToString(),
                Sex = reader.IsDBNull(reader.GetOrdinal("Sex")) ? null : reader["Sex"].ToString(),
                Age = reader.IsDBNull(reader.GetOrdinal("Age")) ? (int?)null : reader.GetInt32("Age"),
                BloodGroup = reader.IsDBNull(reader.GetOrdinal("BloodGroup")) ? null : reader["BloodGroup"].ToString(),
                HeightCm = reader.IsDBNull(reader.GetOrdinal("HeightCm")) ? (decimal?)null : reader.GetDecimal("HeightCm"),
                WeightKg = reader.IsDBNull(reader.GetOrdinal("WeightKg")) ? (decimal?)null : reader.GetDecimal("WeightKg"),
                BMI = reader.IsDBNull(reader.GetOrdinal("BMI")) ? (decimal?)null : reader.GetDecimal("BMI"),
                Temperature = reader.IsDBNull(reader.GetOrdinal("Temperature")) ? (decimal?)null : reader.GetDecimal("Temperature"),
                BP = reader.IsDBNull(reader.GetOrdinal("BP")) ? null : reader["BP"].ToString(),
                SPO2 = reader.IsDBNull(reader.GetOrdinal("SPO2")) ? (decimal?)null : reader.GetDecimal("SPO2"),
                PulseRate = reader.IsDBNull(reader.GetOrdinal("PulseRate")) ? (int?)null : reader.GetInt32("PulseRate"),
                ConsultBy = reader.IsDBNull(reader.GetOrdinal("ConsultBy")) ? null : reader["ConsultBy"].ToString(),
                ReferredBy = reader.IsDBNull(reader.GetOrdinal("ReferredBy")) ? null : reader["ReferredBy"].ToString(),
                Disease = reader.IsDBNull(reader.GetOrdinal("Disease")) ? null : reader["Disease"].ToString(),
                Remarks = reader.IsDBNull(reader.GetOrdinal("Remarks")) ? null : reader["Remarks"].ToString(),
                TotalAmount = reader.IsDBNull(reader.GetOrdinal("TotalAmount")) ? (decimal?)null : reader.GetDecimal("TotalAmount"),
                NextVisitAfter = reader.IsDBNull(reader.GetOrdinal("NextVisitAfter")) ? (int?)null : reader.GetInt32("NextVisitAfter"),
                NextVisitDate = reader.IsDBNull(reader.GetOrdinal("NextVisitDate")) ? (DateTime?)null : reader.GetDateTime("NextVisitDate")
            };
        }

        private static void AddMySqlParameters(MySqlCommand cmd, PrescriptionViewModel p)
        {
            cmd.Parameters.AddWithValue("@EntryType", p.EntryType ?? "");
            cmd.Parameters.AddWithValue("@EntryNo", p.EntryNo ?? "");
            cmd.Parameters.AddWithValue("@EntryDate", p.EntryDate);
            cmd.Parameters.AddWithValue("@UHID", p.UHID ?? "");
            cmd.Parameters.AddWithValue("@PatientName", p.PatientName ?? "");
            cmd.Parameters.AddWithValue("@Address", p.Address ?? "");
            cmd.Parameters.AddWithValue("@Place", p.Place ?? "");
            cmd.Parameters.AddWithValue("@CellNo", p.CellNo ?? "");
            cmd.Parameters.AddWithValue("@Sex", p.Sex ?? "");
            cmd.Parameters.AddWithValue("@Age", p.Age ?? 0);
            cmd.Parameters.AddWithValue("@BloodGroup", p.BloodGroup ?? "");
            cmd.Parameters.AddWithValue("@HeightCm", p.HeightCm ?? 0);
            cmd.Parameters.AddWithValue("@WeightKg", p.WeightKg ?? 0);
            cmd.Parameters.AddWithValue("@BMI", p.BMI ?? 0);
            cmd.Parameters.AddWithValue("@Temperature", p.Temperature ?? 0);
            cmd.Parameters.AddWithValue("@BP", p.BP ?? "");
            cmd.Parameters.AddWithValue("@SPO2", p.SPO2 ?? 0);
            cmd.Parameters.AddWithValue("@PulseRate", p.PulseRate ?? 0);
            cmd.Parameters.AddWithValue("@ConsultBy", p.ConsultBy ?? "");
            cmd.Parameters.AddWithValue("@ReferredBy", p.ReferredBy ?? "");
            cmd.Parameters.AddWithValue("@Disease", p.Disease ?? "");
            cmd.Parameters.AddWithValue("@Remarks", p.Remarks ?? "");
            cmd.Parameters.AddWithValue("@TotalAmount", p.TotalAmount ?? 0);
            cmd.Parameters.AddWithValue("@NextVisitAfter", p.NextVisitAfter ?? 0);
            cmd.Parameters.AddWithValue("@NextVisitDate", p.NextVisitDate);
        }

        private static void AddPrescriptionDetailParameters(MySqlCommand cmd, PrescriptionDetailViewModel detail, int prescriptionId)
        {
            cmd.Parameters.AddWithValue("@PrescriptionId", prescriptionId);
            cmd.Parameters.AddWithValue("@Description", detail.Description ?? "");
            cmd.Parameters.AddWithValue("@Amount", detail.Amount ?? 0);
        }


        private static List<PrescriptionDetailViewModel> GetPrescriptionDetails(int prescriptionId, MySqlConnection conn)
        {
            var details = new List<PrescriptionDetailViewModel>();

            var cmd = new MySqlCommand("SELECT * FROM PrescriptionDetail WHERE PrescriptionId=@PrescriptionId", conn);
            cmd.Parameters.AddWithValue("@PrescriptionId", prescriptionId);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                details.Add(new PrescriptionDetailViewModel
                {
                    PrescriptionDetailId = reader.IsDBNull(reader.GetOrdinal("PrescriptionDetailId")) ? 0 : reader.GetInt32("PrescriptionDetailId"),
                    Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader["Description"].ToString(),
                    Amount = reader.IsDBNull(reader.GetOrdinal("Amount")) ? (decimal?)null : reader.GetDecimal("Amount")
                });
            }

            return details;
        }

        #endregion
    }
}
