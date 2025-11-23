using System;
using System.Collections.Generic;
using System.Data;
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
                    UHIDNo = reader["UHIDNo"]?.ToString(),
                    PhotoBase64 = reader["PhotoBase64"]?.ToString() // load Base64 for preview
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
            var model = GetPatientById(id);
            if (model == null) return NotFound();
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
            cmd.Parameters.AddWithValue("@p_Address", model.Address ?? "");
            cmd.Parameters.AddWithValue("@p_Place", model.Place ?? "");
            cmd.Parameters.AddWithValue("@p_District", model.District ?? "");
            cmd.Parameters.AddWithValue("@p_GSTState", model.GSTState ?? "");
            cmd.Parameters.AddWithValue("@p_Country", model.Country ?? "");
            cmd.Parameters.AddWithValue("@p_PinCode", model.PinCode ?? "");
            cmd.Parameters.AddWithValue("@p_PatientAadhar", model.PatientAadhar ?? "");
            cmd.Parameters.AddWithValue("@p_GuardianAadhar", model.GuardianAadhar ?? "");
            cmd.Parameters.AddWithValue("@p_MobileNo", model.MobileNo ?? "");
            cmd.Parameters.AddWithValue("@p_Email", model.Email ?? "");
            cmd.Parameters.AddWithValue("@p_Occupation", model.Occupation ?? "");
            cmd.Parameters.AddWithValue("@p_MaritalStatus", model.MaritalStatus ?? "");
            cmd.Parameters.AddWithValue("@p_BloodGroup", model.BloodGroup ?? "");
            cmd.Parameters.AddWithValue("@p_AllergicTo", model.AllergicTo ?? "");
            cmd.Parameters.AddWithValue("@p_IsActive", model.IsActive);

            // ================== PHOTO ==================
            if (!string.IsNullOrEmpty(model.PhotoBase64))
            {
                var rawBase64 = model.PhotoBase64.Split(',').Last();
                cmd.Parameters.AddWithValue("@p_Photo", Convert.FromBase64String(rawBase64));
                cmd.Parameters.AddWithValue("@p_PhotoBase64", rawBase64);
            }
            else
            {
                cmd.Parameters.AddWithValue("@p_Photo", DBNull.Value);
                cmd.Parameters.AddWithValue("@p_PhotoBase64", DBNull.Value);
            }

            cmd.Parameters.AddWithValue("@p_PhotoFileName", model.PhotoFileName ?? "");
            cmd.Parameters.AddWithValue("@p_UpdatedBy", User.Identity?.Name ?? "System");

            cmd.ExecuteNonQuery();
            return RedirectToAction(nameof(Index));
        }

        // ================== DELETE ==================
        public IActionResult Delete(int id)
        {
            var model = GetPatientById(id);
            if (model == null) return NotFound();
            return View(model);
        }

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

        // ================== DETAILS ==================
        public IActionResult Details(int id)
        {
            var model = GetPatientById(id);
            if (model == null) return NotFound();
            return View(model);
        }

        // ================== HELPER METHODS ==================
        private PatientRegistrationViewModel GetPatientById(int id)
        {
            var model = new PatientRegistrationViewModel();
            using var conn = _db.GetConnection();
            conn.Open();
            string sql = @"SELECT * FROM patients_master WHERE PatientId = @PatientId";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@PatientId", id);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return null;

            model.PatientId = reader.GetInt32("PatientId");
            model.UHIDType = reader["UHIDType"]?.ToString();
            model.UHIDNo = reader["UHIDNo"]?.ToString();
            model.PatientTitle = reader["PatientTitle"]?.ToString();
            model.PatientName = reader["PatientName"]?.ToString();
            model.DOB = reader["DOB"] as DateTime?;
            model.Sex = reader["Sex"]?.ToString();
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
            model.PhotoFileName = reader["PhotoFileName"]?.ToString();
            model.PhotoBase64 = reader["PhotoBase64"]?.ToString(); // load for Edit display
            return model;
        }

        private int InsertPatientMaster(PatientRegistrationViewModel model)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand("spInsertPatient", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            // All other parameters preserved
            cmd.Parameters.AddWithValue("@p_UHIDType", model.UHIDType);
            cmd.Parameters.AddWithValue("@p_UHIDNo", model.UHIDNo ?? "");
            cmd.Parameters.AddWithValue("@p_PatientTitle", model.PatientTitle);
            cmd.Parameters.AddWithValue("@p_PatientName", model.PatientName);
            cmd.Parameters.AddWithValue("@p_DOB", model.DOB ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@p_Sex", model.Sex);
            cmd.Parameters.AddWithValue("@p_GuardianTitle", model.GuardianTitle ?? "");
            cmd.Parameters.AddWithValue("@p_Guardian", model.Guardian ?? "");
            cmd.Parameters.AddWithValue("@p_Address", model.Address ?? "");
            cmd.Parameters.AddWithValue("@p_Place", model.Place ?? "");
            cmd.Parameters.AddWithValue("@p_District", model.District ?? "");
            cmd.Parameters.AddWithValue("@p_GSTState", model.GSTState ?? "");
            cmd.Parameters.AddWithValue("@p_Country", model.Country ?? "");
            cmd.Parameters.AddWithValue("@p_PinCode", model.PinCode ?? "");
            cmd.Parameters.AddWithValue("@p_PatientAadhar", model.PatientAadhar ?? "");
            cmd.Parameters.AddWithValue("@p_GuardianAadhar", model.GuardianAadhar ?? "");
            cmd.Parameters.AddWithValue("@p_MobileNo", model.MobileNo ?? "");
            cmd.Parameters.AddWithValue("@p_Email", model.Email ?? "");
            cmd.Parameters.AddWithValue("@p_Occupation", model.Occupation ?? "");
            cmd.Parameters.AddWithValue("@p_MaritalStatus", model.MaritalStatus ?? "");
            cmd.Parameters.AddWithValue("@p_BloodGroup", model.BloodGroup ?? "");
            cmd.Parameters.AddWithValue("@p_AllergicTo", model.AllergicTo ?? "");
            cmd.Parameters.AddWithValue("@p_IsActive", model.IsActive);
            cmd.Parameters.AddWithValue("@p_CreatedBy", User.Identity?.Name ?? "System");

            // ================== PHOTO ==================
            if (!string.IsNullOrEmpty(model.PhotoBase64))
            {
                var rawBase64 = model.PhotoBase64.Split(',').Last();
                cmd.Parameters.AddWithValue("@p_Photo", Convert.FromBase64String(rawBase64));
                cmd.Parameters.AddWithValue("@p_PhotoBase64", rawBase64);
            }
            else
            {
                cmd.Parameters.AddWithValue("@p_Photo", DBNull.Value);
                cmd.Parameters.AddWithValue("@p_PhotoBase64", DBNull.Value);
            }
            cmd.Parameters.AddWithValue("@p_PhotoFileName", model.PhotoFileName ?? "");

            var outputParam = new MySqlParameter("@p_NewPatientId", MySqlDbType.Int32)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outputParam);

            cmd.ExecuteNonQuery();
            return Convert.ToInt32(outputParam.Value);
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
            // ================== All dropdown code unchanged ==================
            // Sex
            model.Sexes = new List<SelectListItem>
            {
                new SelectListItem { Text = "Male", Value = "Male" },
                new SelectListItem { Text = "Female", Value = "Female" },
                new SelectListItem { Text = "Other", Value = "Other" }
            };

            // Marital Status
            model.MaritalStatuses = new List<SelectListItem>
            {
                new SelectListItem { Text = "Single", Value = "Single" },
                new SelectListItem { Text = "Married", Value = "Married" },
                new SelectListItem { Text = "Other", Value = "Other" }
            };

            // Blood Group
            model.BloodGroups = new List<SelectListItem>
            {
                new SelectListItem { Text = "A+", Value = "A+" },
                new SelectListItem { Text = "A-", Value = "A-" },
                new SelectListItem { Text = "B+", Value = "B+" },
                new SelectListItem { Text = "B-", Value = "B-" },
                new SelectListItem { Text = "O+", Value = "O+" },
                new SelectListItem { Text = "O-", Value = "O-" },
                new SelectListItem { Text = "AB+", Value = "AB+" },
                new SelectListItem { Text = "AB-", Value = "AB-" }
            };

            // UHID Types
            model.UHIDTypes = new List<SelectListItem>
            {
                new SelectListItem { Text = "Patient Registration", Value = "Patient Registration" },
                new SelectListItem { Text = "OPD", Value = "OPD" }
            };

            // Patient Titles
            model.PatientTitles = new List<SelectListItem>
            {
                new SelectListItem { Text = "Mr", Value = "Mr" },
                new SelectListItem { Text = "Mrs", Value = "Mrs" },
                new SelectListItem { Text = "Miss", Value = "Miss" },
                new SelectListItem { Text = "Master", Value = "Master" }
            };

            // Guardian Titles
            model.GuardianTitles = new List<SelectListItem>
            {
                new SelectListItem { Text = "S/O", Value = "S/O" },
                new SelectListItem { Text = "D/O", Value = "D/O" },
                new SelectListItem { Text = "F/O", Value = "F/O" },
                new SelectListItem { Text = "M/O", Value = "M/O" }
            };

            // GST States
            model.GSTStates = new List<SelectListItem>
            {
                new SelectListItem { Text = "Tamil Nadu", Value = "33-Tamil Nadu" },
                new SelectListItem { Text = "Kerala", Value = "32-Kerala" }
                // Add more as required
            };

            // Countries
            model.Countries = new List<SelectListItem>
            {
                new SelectListItem { Text = "India", Value = "India" },
                new SelectListItem { Text = "USA", Value = "USA" }
            };

            // Consultant Doctors
            model.ConsultantDoctors = new List<SelectListItem>();
            using var conn = _db.GetConnection();
            conn.Open();
            using (var cmd = new MySqlCommand("SELECT Doctor_Id, Doctor_Name FROM doctor_master WHERE Active=1 ORDER BY Doctor_Name", conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    model.ConsultantDoctors.Add(new SelectListItem
                    {
                        Value = reader["Doctor_Id"].ToString(),
                        Text = reader["Doctor_Name"].ToString()
                    });
                }
            }

            // Referred Doctors
            model.RefDoctors = new List<SelectListItem>();
            using (var cmd = new MySqlCommand("SELECT Doctor_Id, Doctor_Name FROM doctor_master WHERE Active=1 ORDER BY Doctor_Name", conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    model.RefDoctors.Add(new SelectListItem
                    {
                        Value = reader["Doctor_Id"].ToString(),
                        Text = reader["Doctor_Name"].ToString()
                    });
                }
            }

            // Payment Terms
            model.PaymentTermsList = new List<SelectListItem>
            {
                new SelectListItem { Text = "Cash", Value = "Cash" },
                new SelectListItem { Text = "Credit", Value = "Credit" }
            };

            // Registration Types
            model.RegistrationTypes = new List<SelectListItem>
            {
                new SelectListItem { Text = "Walk-in", Value = "Walk-in" },
                new SelectListItem { Text = "Online", Value = "Online" }
            };

            // Comp / Ins / Camp
            model.CompInsCampList = new List<SelectListItem>
            {
                new SelectListItem { Text = "Company", Value = "Company" },
                new SelectListItem { Text = "Insurance", Value = "Insurance" },
                new SelectListItem { Text = "Camp", Value = "Camp" }
            };
        }
    }
}
