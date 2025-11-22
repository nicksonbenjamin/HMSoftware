using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using ClinicApp.ViewModels;
using ClinicApp.Data;
using MySql.Data.MySqlClient;

namespace ClinicApp.Controllers
{
    public class PatientRegistrationController : Controller
    {
        private readonly ILogger<PatientRegistrationController> _logger;
        private readonly ClinicAppMySqlDbContext _db;

        public PatientRegistrationController(ILogger<PatientRegistrationController> logger, ClinicAppMySqlDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        // ================== LIST PATIENTS ==================
        public IActionResult Index()
        {
            var list = new List<PatientRegistrationViewModel>();
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand("spGetAllPatients", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new PatientRegistrationViewModel
                {
                    PatientId = reader.GetInt32("PatientId"),
                    PatientName = reader.GetString("PatientName"),
                    MobileNo = reader.GetString("MobileNo"),
                    Sex = reader.GetString("Sex"),
                    UHIDNo = reader["UHIDNo"]?.ToString()
                });
            }
            return View(list);
        }

        // ================== CREATE GET ==================
        public IActionResult Create()
        {
            var model = new PatientRegistrationViewModel
            {
                RegistrationDate = DateTime.Now.Date,
                RegistrationTime = DateTime.Now.TimeOfDay,
                IsActive = true
            };
            PopulateDropdowns(model);
            return View(model);
        }

        // ================== CREATE POST ==================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PatientRegistrationViewModel model)
        {
            // if (!ModelState.IsValid)
            // {
            //     PopulateDropdowns(model);
            //     return View(model);
            // }

            int patientId = InsertPatientMaster(model);
            if (patientId <= 0)
            {
                ModelState.AddModelError("", "Error inserting patient.");
                PopulateDropdowns(model);
                return View(model);
            }

            bool entry = InsertPatientEntry(model, patientId);
            if (!entry)
            {
                ModelState.AddModelError("", "Patient saved, entry failed.");
                PopulateDropdowns(model);
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        // ================== EDIT GET ==================
        public IActionResult Edit(int id)
        {
            var model = new PatientRegistrationViewModel();
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand("spGetPatientById", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@p_PatientId", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                model.PatientId = reader.GetInt32("PatientId");
                model.UHIDType = reader.GetString("UHIDType");
                model.UHIDNo = reader["UHIDNo"]?.ToString();
                model.PatientTitle = reader.GetString("PatientTitle");
                model.PatientName = reader.GetString("PatientName");
                model.DOB = reader["DOB"] == DBNull.Value ? null : (DateTime?)reader["DOB"];
                model.Sex = reader.GetString("Sex");
                model.GuardianTitle = reader["GuardianTitle"]?.ToString();
                model.Guardian = reader["Guardian"]?.ToString();
                model.Address = reader.GetString("Address");
                model.Place = reader["Place"]?.ToString();
                model.District = reader["District"]?.ToString();
                model.GSTState = reader["GSTState"]?.ToString();
                model.Country = reader["Country"]?.ToString();
                model.PinCode = reader["PinCode"]?.ToString();
                model.PatientAadhar = reader["PatientAadhar"]?.ToString();
                model.GuardianAadhar = reader["GuardianAadhar"]?.ToString();
                model.MobileNo = reader["MobileNo"]?.ToString();
                model.Email = reader["Email"]?.ToString();
                model.Occupation = reader["Occupation"]?.ToString();
                model.MaritalStatus = reader["MaritalStatus"]?.ToString();
                model.BloodGroup = reader["BloodGroup"]?.ToString();
                model.AllergicTo = reader["AllergicTo"]?.ToString();
                model.IsActive = reader["IsActive"] != DBNull.Value && Convert.ToBoolean(reader["IsActive"]);

                // patient_entry fields
                model.RegistrationDate = reader["RegistrationDate"] == DBNull.Value ? DateTime.Now.Date : Convert.ToDateTime(reader["RegistrationDate"]);
                model.RegistrationTime = reader["RegistrationTime"] == DBNull.Value ? DateTime.Now.TimeOfDay : (TimeSpan)reader["RegistrationTime"];
                model.Age = reader["Age"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Age"]);
                model.ConsultantDoctorId = reader["ConsultantDoctorId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ConsultantDoctorId"]);
                model.RefDoctorId = reader["RefDoctorId"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["RefDoctorId"]);
                model.PaymentTerms = reader["PaymentTerms"]?.ToString();
                model.RegistrationType = reader["RegistrationType"]?.ToString();
                model.CompOrInsOrCamp = reader["CompOrInsOrCamp"]?.ToString();
                model.PatientCondition = reader["PatientCondition"]?.ToString();
                model.ReferenceOrPicmeNo = reader["ReferenceOrPicmeNo"]?.ToString();
            }

            PopulateDropdowns(model);
            return View(model);
        }

        // ================== EDIT POST ==================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PatientRegistrationViewModel model)
        {
            // if (!ModelState.IsValid)
            // {
            //     PopulateDropdowns(model);
            //     return View(model);
            // }

            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand("spUpdatePatient", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@p_PatientId", model.PatientId);
            cmd.Parameters.AddWithValue("@p_UHIDType", model.UHIDType);
            cmd.Parameters.AddWithValue("@p_UHIDNo", model.UHIDNo ?? "");
            cmd.Parameters.AddWithValue("@p_PatientTitle", model.PatientTitle);
            cmd.Parameters.AddWithValue("@p_PatientName", model.PatientName);
            cmd.Parameters.AddWithValue("@p_DOB", model.DOB ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@p_Sex", model.Sex);
            cmd.Parameters.AddWithValue("@p_GuardianTitle", model.GuardianTitle ?? "");
            cmd.Parameters.AddWithValue("@p_Guardian", model.Guardian ?? "");
            cmd.Parameters.AddWithValue("@p_Address", model.Address);
            cmd.Parameters.AddWithValue("@p_Place", model.Place ?? "");
            cmd.Parameters.AddWithValue("@p_District", model.District ?? "");
            cmd.Parameters.AddWithValue("@p_GSTState", model.GSTState ?? "");
            cmd.Parameters.AddWithValue("@p_Country", model.Country ?? "");
            cmd.Parameters.AddWithValue("@p_PinCode", model.PinCode ?? "");
            cmd.Parameters.AddWithValue("@p_PatientAadhar", model.PatientAadhar ?? "");
            cmd.Parameters.AddWithValue("@p_GuardianAadhar", model.GuardianAadhar ?? "");
            cmd.Parameters.AddWithValue("@p_MobileNo", model.MobileNo ?? "");
            cmd.Parameters.AddWithValue("@p_Email", model.Email ?? "");

            // Convert photo
            byte[] photoBytes = null;
            if (model.Photo != null)
            {
                using var ms = new MemoryStream();
                model.Photo.CopyTo(ms);
                photoBytes = ms.ToArray();
            }
            cmd.Parameters.AddWithValue("@p_Photo", (object)photoBytes ?? DBNull.Value);

            cmd.Parameters.AddWithValue("@p_Occupation", model.Occupation ?? "");
            cmd.Parameters.AddWithValue("@p_MaritalStatus", model.MaritalStatus ?? "");
            cmd.Parameters.AddWithValue("@p_BloodGroup", model.BloodGroup ?? "");
            cmd.Parameters.AddWithValue("@p_AllergicTo", model.AllergicTo ?? "");
            cmd.Parameters.AddWithValue("@p_IsActive", model.IsActive);
            cmd.Parameters.AddWithValue("@p_UpdatedBy", User.Identity?.Name ?? "System");

            cmd.ExecuteNonQuery();
            return RedirectToAction(nameof(Index));
        }

        // ================== HELPER METHODS ==================
        private int InsertPatientMaster(PatientRegistrationViewModel model)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand("spInsertPatient", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@p_UHIDType", model.UHIDType);
            cmd.Parameters.AddWithValue("@p_UHIDNo", model.UHIDNo ?? "");
            cmd.Parameters.AddWithValue("@p_PatientTitle", model.PatientTitle);
            cmd.Parameters.AddWithValue("@p_PatientName", model.PatientName);
            cmd.Parameters.AddWithValue("@p_DOB", model.DOB ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@p_Sex", model.Sex);
            cmd.Parameters.AddWithValue("@p_GuardianTitle", model.GuardianTitle ?? "");
            cmd.Parameters.AddWithValue("@p_Guardian", model.Guardian ?? "");
            cmd.Parameters.AddWithValue("@p_Address", model.Address);
            cmd.Parameters.AddWithValue("@p_Place", model.Place ?? "");
            cmd.Parameters.AddWithValue("@p_District", model.District ?? "");
            cmd.Parameters.AddWithValue("@p_GSTState", model.GSTState ?? "");
            cmd.Parameters.AddWithValue("@p_Country", model.Country ?? "");
            cmd.Parameters.AddWithValue("@p_PinCode", model.PinCode ?? "");
            cmd.Parameters.AddWithValue("@p_PatientAadhar", model.PatientAadhar ?? "");
            cmd.Parameters.AddWithValue("@p_GuardianAadhar", model.GuardianAadhar ?? "");
            cmd.Parameters.AddWithValue("@p_MobileNo", model.MobileNo ?? "");
            cmd.Parameters.AddWithValue("@p_Email", model.Email ?? "");

            byte[] photoBytes = null;
            if (model.Photo != null)
            {
                using var ms = new MemoryStream();
                model.Photo.CopyTo(ms);
                photoBytes = ms.ToArray();
            }
            cmd.Parameters.AddWithValue("@p_Photo", (object)photoBytes ?? DBNull.Value);

            cmd.Parameters.AddWithValue("@p_Occupation", model.Occupation ?? "");
            cmd.Parameters.AddWithValue("@p_MaritalStatus", model.MaritalStatus ?? "");
            cmd.Parameters.AddWithValue("@p_BloodGroup", model.BloodGroup ?? "");
            cmd.Parameters.AddWithValue("@p_AllergicTo", model.AllergicTo ?? "");
            cmd.Parameters.AddWithValue("@p_IsActive", model.IsActive);
            cmd.Parameters.AddWithValue("@p_CreatedBy", User.Identity?.Name ?? "System");

            var outParam = new MySqlParameter("@p_NewPatientId", MySqlDbType.Int32)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outParam);

            cmd.ExecuteNonQuery();
            return Convert.ToInt32(outParam.Value);
        }
// GET: PatientRegistration/Delete/5
// public IActionResult Delete(int id)
// {
//     using var conn = _db.GetConnection();
//     conn.Open();
//     using var cmd = new MySqlCommand("spDeletePatient", conn)
//     {
//         CommandType = CommandType.StoredProcedure
//     };
//     cmd.Parameters.AddWithValue("@p_PatientId", id);
//     cmd.ExecuteNonQuery();

//     return RedirectToAction(nameof(Index));
// }

// GET: PatientRegistration/Delete/5
public IActionResult Delete(int id)
{
    var model = new PatientRegistrationViewModel();

    using var conn = _db.GetConnection();
    conn.Open();

    // Get patient details
    using var cmd = new MySqlCommand("SELECT pm.*, pe.RegistrationDate, pe.RegistrationTime, pe.Age, pe.ConsultantDoctorId, pe.RefDoctorId, pe.PaymentTerms, pe.RegistrationType, pe.CompOrInsOrCamp, pe.PatientCondition, pe.ReferenceOrPicmeNo FROM patients_master pm LEFT JOIN patient_entry pe ON pm.PatientId = pe.PatientId WHERE pm.PatientId = @PatientId", conn);
    cmd.Parameters.AddWithValue("@PatientId", id);

    using var reader = cmd.ExecuteReader();
    if (reader.Read())
    {
        model.PatientId = reader.GetInt32("PatientId");
        model.UHIDType = reader.GetString("UHIDType");
        model.UHIDNo = reader["UHIDNo"]?.ToString();
        model.PatientTitle = reader.GetString("PatientTitle");
        model.PatientName = reader.GetString("PatientName");
        model.DOB = reader["DOB"] as DateTime?;
        model.Sex = reader.GetString("Sex");
        model.GuardianTitle = reader["GuardianTitle"]?.ToString();
        model.Guardian = reader["Guardian"]?.ToString();
        model.Address = reader["Address"].ToString();
        model.Place = reader["Place"]?.ToString();
        model.District = reader["District"]?.ToString();
        model.GSTState = reader["GSTState"]?.ToString();
        model.Country = reader["Country"]?.ToString();
        model.PinCode = reader["PinCode"]?.ToString();
        model.PatientAadhar = reader["PatientAadhar"]?.ToString();
        model.GuardianAadhar = reader["GuardianAadhar"]?.ToString();
        model.MobileNo = reader["MobileNo"]?.ToString();
        model.Email = reader["Email"]?.ToString();
        model.Occupation = reader["Occupation"]?.ToString();
        model.MaritalStatus = reader["MaritalStatus"]?.ToString();
        model.BloodGroup = reader["BloodGroup"]?.ToString();
        model.AllergicTo = reader["AllergicTo"]?.ToString();
        model.IsActive = Convert.ToBoolean(reader["IsActive"]);
        model.RegistrationDate = reader["RegistrationDate"] as DateTime? ?? DateTime.Now;
        model.RegistrationTime = reader["RegistrationTime"] as TimeSpan? ?? TimeSpan.Zero;
        model.Age = reader["Age"] as int? ?? 0;
        model.ConsultantDoctorId = reader["ConsultantDoctorId"] as int? ?? 0;
        model.RefDoctorId = reader["RefDoctorId"] as int?;
        model.PaymentTerms = reader["PaymentTerms"]?.ToString();
        model.RegistrationType = reader["RegistrationType"]?.ToString();
        model.CompOrInsOrCamp = reader["CompOrInsOrCamp"]?.ToString();
        model.PatientCondition = reader["PatientCondition"]?.ToString();
        model.ReferenceOrPicmeNo = reader["ReferenceOrPicmeNo"]?.ToString();
    }
    else
    {
        return NotFound();
    }

    return View(model);
}

// POST: PatientRegistration/Delete/5
[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult DeleteConfirmed(int id)
{
    using var conn = _db.GetConnection();
    conn.Open();
    using var cmd = new MySqlCommand("spDeletePatient", conn)
    {
        CommandType = CommandType.StoredProcedure
    };
    cmd.Parameters.AddWithValue("@p_PatientId", id);
    cmd.ExecuteNonQuery();

    return RedirectToAction(nameof(Index));
}

        private bool InsertPatientEntry(PatientRegistrationViewModel model, int patientId)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand("spInsertPatientEntry", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@p_PatientId", patientId);
            cmd.Parameters.AddWithValue("@p_RegistrationDate", model.RegistrationDate);
            cmd.Parameters.AddWithValue("@p_RegistrationTime", model.RegistrationTime);
            cmd.Parameters.AddWithValue("@p_Age", model.Age);
            cmd.Parameters.AddWithValue("@p_ConsultantDoctorId", model.ConsultantDoctorId);
            cmd.Parameters.AddWithValue("@p_RefDoctorId", model.RefDoctorId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@p_PaymentTerms", model.PaymentTerms ?? "");
            cmd.Parameters.AddWithValue("@p_RegistrationType", model.RegistrationType ?? "");
            cmd.Parameters.AddWithValue("@p_CompOrInsOrCamp", model.CompOrInsOrCamp ?? "");
            cmd.Parameters.AddWithValue("@p_PatientCondition", model.PatientCondition ?? "");
            cmd.Parameters.AddWithValue("@p_ReferenceOrPicmeNo", model.ReferenceOrPicmeNo ?? "");
            cmd.Parameters.AddWithValue("@p_CreatedBy", User.Identity?.Name ?? "System");

            return cmd.ExecuteNonQuery() > 0;
        }

        private void PopulateDropdowns(PatientRegistrationViewModel model)
        {
            model.UHIDTypes = new List<SelectListItem>
            {
                new("Patient Registration","Patient Registration"),
                new("Other UHID","Other UHID")
            };

            model.Sexes = new List<SelectListItem>
            {
                new("Male","Male"),
                new("Female","Female"),
                new("Other","Other")
            };

            model.PatientTitles = new List<SelectListItem>
            {
                new("Mr","Mr"),
                new("Mrs","Mrs"),
                new("Miss","Miss")
            };

            model.GuardianTitles = new List<SelectListItem>
            {
                new("F/O","F/O"),
                new("M/O","M/O"),
                new("S/O","S/O")
            };

            model.GSTStates = new List<SelectListItem>
            {
                new("33-Tamil Nadu","33-Tamil Nadu"),
                new("31-Karnataka","31-Karnataka")
            };

            model.Countries = new List<SelectListItem>
            {
                new("India","India"),
                new("USA","USA")
            };

            model.MaritalStatuses = new List<SelectListItem>
            {
                new("Single","Single"),
                new("Married","Married")
            };

            model.BloodGroups = new List<SelectListItem>
            {
                new("A+","A+"), new("A-","A-"),
                new("B+","B+"), new("B-","B-"),
                new("O+","O+"), new("O-","O-"),
                new("AB+","AB+"), new("AB-","AB-")
            };

            using var conn = _db.GetConnection();
            conn.Open();
            string sql = "SELECT doctor_id, doctor_name FROM doctor_master WHERE active = 1 ORDER BY doctor_name";
            using var cmd = new MySqlCommand(sql, conn);
            using var dr = cmd.ExecuteReader();

            var list = new List<SelectListItem> { new("-- Select Doctor --", "") };
            while (dr.Read())
            {
                list.Add(new SelectListItem(
                    dr.GetString("doctor_name"),
                    dr.GetInt32("doctor_id").ToString()
                ));
            }
            model.ConsultantDoctors = list;
            model.RefDoctors = new List<SelectListItem>(list);

            model.PaymentTermsList = new List<SelectListItem>
            {
                new("Cash","Cash"),
                new("Card","Card"),
                new("Insurance","Insurance")
            };

            model.RegistrationTypes = new List<SelectListItem>
            {
                new("Normal","Normal"),
                new("Emergency","Emergency"),
                new("Camp","Camp")
            };

            model.CompInsCampList = new List<SelectListItem>
            {
                new("None","None"),
                new("Star Health","Star Health"),
                new("EHS","EHS"),
                new("Camp","Camp")
            };
        }

        // ================== DETAILS ==================
public IActionResult Details(int id)
{
    var model = new PatientRegistrationViewModel();

    using var conn = _db.GetConnection();
    conn.Open();

    string sql =
        "SELECT pm.*, pe.RegistrationDate, pe.RegistrationTime, pe.Age, pe.ConsultantDoctorId, " +
        "pe.RefDoctorId, pe.PaymentTerms, pe.RegistrationType, pe.CompOrInsOrCamp, pe.PatientCondition, " +
        "pe.ReferenceOrPicmeNo " +
        "FROM patients_master pm " +
        "LEFT JOIN patient_entry pe ON pm.PatientId = pe.PatientId " +
        "WHERE pm.PatientId = @PatientId";

    using var cmd = new MySqlCommand(sql, conn);
    cmd.Parameters.AddWithValue("@PatientId", id);

    using var reader = cmd.ExecuteReader();
    if (!reader.Read())
        return NotFound();

    model.PatientId = reader.GetInt32("PatientId");
    model.UHIDType = reader.GetString("UHIDType");
    model.UHIDNo = reader["UHIDNo"]?.ToString();
    model.PatientTitle = reader.GetString("PatientTitle");
    model.PatientName = reader.GetString("PatientName");
    model.DOB = reader["DOB"] as DateTime?;
    model.Sex = reader.GetString("Sex");
    model.GuardianTitle = reader["GuardianTitle"]?.ToString();
    model.Guardian = reader["Guardian"]?.ToString();
    model.Address = reader["Address"]?.ToString();
    model.Place = reader["Place"]?.ToString();
    model.District = reader["District"]?.ToString();
    model.GSTState = reader["GSTState"]?.ToString();
    model.Country = reader["Country"]?.ToString();
    model.PinCode = reader["PinCode"]?.ToString();
    model.PatientAadhar = reader["PatientAadhar"]?.ToString();
    model.GuardianAadhar = reader["GuardianAadhar"]?.ToString();
    model.MobileNo = reader["MobileNo"]?.ToString();
    model.Email = reader["Email"]?.ToString();
    model.Occupation = reader["Occupation"]?.ToString();
    model.MaritalStatus = reader["MaritalStatus"]?.ToString();
    model.BloodGroup = reader["BloodGroup"]?.ToString();
    model.AllergicTo = reader["AllergicTo"]?.ToString();

    model.IsActive = reader["IsActive"] != DBNull.Value && Convert.ToBoolean(reader["IsActive"]);
    model.RegistrationDate = reader["RegistrationDate"] as DateTime? ?? DateTime.Now;
    model.RegistrationTime = reader["RegistrationTime"] as TimeSpan? ?? TimeSpan.Zero;
    model.Age = reader["Age"] as int? ?? 0;
    model.ConsultantDoctorId = reader["ConsultantDoctorId"] as int? ?? 0;
    model.RefDoctorId = reader["RefDoctorId"] as int?;
    model.PaymentTerms = reader["PaymentTerms"]?.ToString();
    model.RegistrationType = reader["RegistrationType"]?.ToString();
    model.CompOrInsOrCamp = reader["CompOrInsOrCamp"]?.ToString();
    model.PatientCondition = reader["PatientCondition"]?.ToString();
    model.ReferenceOrPicmeNo = reader["ReferenceOrPicmeNo"]?.ToString();

    return View(model);
}

    }
}
