using Microsoft.AspNetCore.Mvc; 
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using ClinicApp.ViewModels;
using ClinicApp.Models;
using Microsoft.Extensions.Configuration;
using Rotativa.AspNetCore;

namespace ClinicApp.Controllers
{
    public class DoctorPrescriptionController : Controller
    {
        private readonly IConfiguration _config;

        public DoctorPrescriptionController(IConfiguration config)
        {
            _config = config;
        }

        private MySqlConnection GetConn() =>
            new MySqlConnection(_config.GetConnectionString("DefaultConnection"));

        #region Index
        public IActionResult Index()
        {
            var list = new List<DoctorPrescriptionListVM>();

            using var con = GetConn();
            con.Open();

            using var cmd = new MySqlCommand("CALL sp_GetPrescriptionList();", con);
            using var dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                list.Add(new DoctorPrescriptionListVM
                {
                    DrPrescriptionId = dr.GetInt32("DrPrescriptionId"),
                    PatientName = dr.GetString("PatientName"),
                    EntryType = dr.GetString("EntryType"),
                    EntryNumber = dr.GetInt32("EntryNumber"),
                    PrescriptionDate = dr.GetDateTime("PrescriptionDate"),
                    NextVisitDate = dr["NextVisitDate"] != DBNull.Value ? dr.GetDateTime("NextVisitDate") : null,
                    DiseaseName = dr["Desease"] != DBNull.Value ? dr.GetString("Desease") : null
                });
            }

            return View(list);
        }
        #endregion

        #region Create
        public IActionResult Create()
        {
            var vm = new DoctorPrescriptionVM
            {
                Prescription = new DrPrescription { EntryNumber = 0 },
                Medicines = new List<DrPrescriptionDetails>(),
                Clinical = new List<DrPrescriptionClinical>(),
                PatientList = LoadPatients(),
                DiseaseList = LoadDiseases(),
                DosePatternList = LoadDosePatterns(),
                MedicineList = LoadMedicines(),
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult Save(DoctorPrescriptionVM model)
        {
            int newId;
            using var con = GetConn();
            con.Open();

            if (model.Prescription.EntryNumber <= 0)
            {
                using var cmdEntry = new MySqlCommand(
                    "SELECT IFNULL(MAX(EntryNumber),0)+1 FROM DrPrescription WHERE EntryType=@EntryType;", con);
                cmdEntry.Parameters.AddWithValue("@EntryType", model.Prescription.EntryType);
                model.Prescription.EntryNumber = Convert.ToInt32(cmdEntry.ExecuteScalar());
            }

            using (var cmd = new MySqlCommand(
                "CALL sp_SavePrescription(@PatientId,@EntryType,@EntryNumber,@EntryPeriod,@DiseaseId,@Height,@Weight,@BMI,@Temp,@BP,@SPO2,@Pulse,@NextVisit);", con))
            {
                cmd.Parameters.AddWithValue("@PatientId", model.Prescription.PatientId);
                cmd.Parameters.AddWithValue("@EntryType", model.Prescription.EntryType);
                cmd.Parameters.AddWithValue("@EntryNumber", model.Prescription.EntryNumber);
                cmd.Parameters.AddWithValue("@EntryPeriod", model.Prescription.EntryPeriod);
                cmd.Parameters.AddWithValue("@DiseaseId", model.Prescription.DeseaseId);
                cmd.Parameters.AddWithValue("@Height", model.Prescription.Height);
                cmd.Parameters.AddWithValue("@Weight", model.Prescription.Weight);
                cmd.Parameters.AddWithValue("@BMI", model.Prescription.BMI);
                cmd.Parameters.AddWithValue("@Temp", model.Prescription.TempInCelcius);
                cmd.Parameters.AddWithValue("@BP", model.Prescription.BP);
                cmd.Parameters.AddWithValue("@SPO2", model.Prescription.SPO2);
                cmd.Parameters.AddWithValue("@Pulse", model.Prescription.PulseRate);
                cmd.Parameters.AddWithValue("@NextVisit", model.Prescription.NextVisitDate);

                newId = Convert.ToInt32(cmd.ExecuteScalar());
            }

            // Save medicines
            if (model.Medicines != null)
            {
                foreach (var m in model.Medicines)
                {
                    using var cmd = new MySqlCommand(
                        "CALL sp_SavePrescriptionMedicine(@DrPrescriptionId,@ProductId,@DosePatternId,@DosePattern,@DoseUsage,@Days,@Qty);",
                        con);

                    cmd.Parameters.AddWithValue("@DrPrescriptionId", newId);
                    cmd.Parameters.AddWithValue("@ProductId", m.ProductId);
                    cmd.Parameters.AddWithValue("@DosePatternId", m.DosePatternId);
                    cmd.Parameters.AddWithValue("@DosePattern", m.DosePattern);
                    cmd.Parameters.AddWithValue("@DoseUsage", m.DoseUsage);
                    cmd.Parameters.AddWithValue("@Days", m.DoseDays);
                    cmd.Parameters.AddWithValue("@Qty", m.Qty);

                    cmd.ExecuteNonQuery();
                }
            }

            // Save clinical notes
            if (model.Clinical != null)
            {
                foreach (var c in model.Clinical)
                {
                    using var cmd = new MySqlCommand(
                        "INSERT INTO DrPrescriptionClinical (DrPrescriptionId, ClinicalNote, Result) VALUES (@Pid, @Note, @Result);",
                        con);
                    cmd.Parameters.AddWithValue("@Pid", newId);
                    cmd.Parameters.AddWithValue("@Note", c.ClinicalNote);
                    cmd.Parameters.AddWithValue("@Result", c.Result ?? "");
                    cmd.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Edit
        public IActionResult Edit(int id)
        {
            var vm = new DoctorPrescriptionVM
            {
                Prescription = GetPrescription(id),
                Medicines = GetPrescriptionDetails(id),
                Clinical = GetClinicalDetails(id),
                PatientList = LoadPatients(),
                DiseaseList = LoadDiseases(),
                DosePatternList = LoadDosePatterns(),
                MedicineList = LoadMedicines()
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult Update(DoctorPrescriptionVM model)
        {
            using var con = GetConn();
            con.Open();

            using (var cmd = new MySqlCommand(
                @"UPDATE DrPrescription SET 
                    PatientId=@PatientId,
                    EntryType=@EntryType,
                    EntryNumber=@EntryNumber,
                    EntryPeriod=@EntryPeriod,
                    DeseaseId=@DiseaseId,
                    Height=@Height,
                    Weight=@Weight,
                    BMI=@BMI,
                    TempInCelcius=@Temp,
                    BP=@BP,
                    SPO2=@SPO2,
                    PulseRate=@Pulse,
                    NextVisitDate=@NextVisit
                  WHERE DrPrescriptionId=@Id;", con))
            {
                cmd.Parameters.AddWithValue("@Id", model.Prescription.DrPrescriptionId);
                cmd.Parameters.AddWithValue("@PatientId", model.Prescription.PatientId);
                cmd.Parameters.AddWithValue("@EntryType", model.Prescription.EntryType);
                cmd.Parameters.AddWithValue("@EntryNumber", model.Prescription.EntryNumber);
                cmd.Parameters.AddWithValue("@EntryPeriod", model.Prescription.EntryPeriod);
                cmd.Parameters.AddWithValue("@DiseaseId", model.Prescription.DeseaseId);
                cmd.Parameters.AddWithValue("@Height", model.Prescription.Height);
                cmd.Parameters.AddWithValue("@Weight", model.Prescription.Weight);
                cmd.Parameters.AddWithValue("@BMI", model.Prescription.BMI);
                cmd.Parameters.AddWithValue("@Temp", model.Prescription.TempInCelcius);
                cmd.Parameters.AddWithValue("@BP", model.Prescription.BP);
                cmd.Parameters.AddWithValue("@SPO2", model.Prescription.SPO2);
                cmd.Parameters.AddWithValue("@Pulse", model.Prescription.PulseRate);
                cmd.Parameters.AddWithValue("@NextVisit", model.Prescription.NextVisitDate);

                cmd.ExecuteNonQuery();
            }

            // Update medicines
            using var delM = new MySqlCommand("DELETE FROM DrPrescriptionDetails WHERE DrPrescriptionId=@Id;", con);
            delM.Parameters.AddWithValue("@Id", model.Prescription.DrPrescriptionId);
            delM.ExecuteNonQuery();

            if (model.Medicines != null)
            {
                foreach (var m in model.Medicines)
                {
                    using var cmd = new MySqlCommand(
                        "CALL sp_SavePrescriptionMedicine(@DrPrescriptionId,@ProductId,@DosePatternId,@DosePattern,@DoseUsage,@Days,@Qty);",
                        con);

                    cmd.Parameters.AddWithValue("@DrPrescriptionId", model.Prescription.DrPrescriptionId);
                    cmd.Parameters.AddWithValue("@ProductId", m.ProductId);
                    cmd.Parameters.AddWithValue("@DosePatternId", m.DosePatternId);
                    cmd.Parameters.AddWithValue("@DosePattern", m.DosePattern);
                    cmd.Parameters.AddWithValue("@DoseUsage", m.DoseUsage);
                    cmd.Parameters.AddWithValue("@Days", m.DoseDays);
                    cmd.Parameters.AddWithValue("@Qty", m.Qty);

                    cmd.ExecuteNonQuery();
                }
            }

            // Update clinical notes
            using var delC = new MySqlCommand("DELETE FROM DrPrescriptionClinical WHERE DrPrescriptionId=@Id;", con);
            delC.Parameters.AddWithValue("@Id", model.Prescription.DrPrescriptionId);
            delC.ExecuteNonQuery();

            if (model.Clinical != null)
            {
                foreach (var c in model.Clinical)
                {
                    using var cmd = new MySqlCommand(
                        "INSERT INTO DrPrescriptionClinical (DrPrescriptionId, ClinicalNote, Result) VALUES (@Pid, @Note, @Result);",
                        con);
                    cmd.Parameters.AddWithValue("@Pid", model.Prescription.DrPrescriptionId);
                    cmd.Parameters.AddWithValue("@Note", c.ClinicalNote);
                    cmd.Parameters.AddWithValue("@Result", c.Result ?? "");
                    cmd.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Delete
        public IActionResult Delete(int id)
        {
            using var con = GetConn();
            con.Open();

            new MySqlCommand("DELETE FROM DrPrescriptionClinical WHERE DrPrescriptionId=@Id;", con)
                { Parameters = { new("@Id", id) } }.ExecuteNonQuery();

            new MySqlCommand("DELETE FROM DrPrescriptionDetails WHERE DrPrescriptionId=@Id;", con)
                { Parameters = { new("@Id", id) } }.ExecuteNonQuery();

            new MySqlCommand("DELETE FROM DrPrescription WHERE DrPrescriptionId=@Id;", con)
                { Parameters = { new("@Id", id) } }.ExecuteNonQuery();

            return RedirectToAction("Index");
        }
        #endregion

        #region Details
        public IActionResult Details(int id)
        {
            var prescription = GetPrescription(id);
            if (prescription == null)
                return NotFound();

            var medicines = GetPrescriptionDetails(id);
            var clinicalNotes = GetClinicalDetails(id);

            var patient = LoadPatients().Find(p => p.PatientId == prescription.PatientId);
            if (patient != null) prescription.PatientName = patient.PatientName;

            var vm = new DoctorPrescriptionVM
            {
                Prescription = prescription,
                Medicines = medicines,
                Clinical = clinicalNotes,
                MedicineList = LoadMedicines(),
                DosePatternList = LoadDosePatterns(),
                DiseaseList = LoadDiseases()
            };

            return View(vm);
        }
        #endregion

        #region Print PDF
        #region Print PDF
public IActionResult PrintPdf(int id)
{
    var prescription = GetPrescription(id);
    if (prescription == null)
        return NotFound();

    var medicines = GetPrescriptionDetails(id);
    var clinicalNotes = GetClinicalDetails(id);

    // Populate Patient Name
    var patient = LoadPatients().FirstOrDefault(p => p.PatientId == prescription.PatientId);
    if (patient != null)
        prescription.PatientName = patient.PatientName;

    var vm = new DoctorPrescriptionVM
    {
        Prescription = prescription,
        Medicines = medicines,
        Clinical = clinicalNotes,
        DiseaseList = LoadDiseases(),
        MedicineList = LoadMedicines(),
        DosePatternList = LoadDosePatterns()
    };

    // Sanitize patient name for file name
    var patientNameSafe = string.Join("_", prescription.PatientName.Split(Path.GetInvalidFileNameChars()));

    // Include patient ID with prefix 'ID' and current datetime
    var dateTimePart = DateTime.Now.ToString("yyyyMMdd_HHmmss");
    var fileName = $"Prescription_ID{prescription.PatientId}_{patientNameSafe}_{dateTimePart}.pdf";

    return new ViewAsPdf("PrintPdf", vm)
    {
        FileName = fileName,
        PageSize = Rotativa.AspNetCore.Options.Size.A4,
        PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
        PageMargins = new Rotativa.AspNetCore.Options.Margins(5, 5, 10, 5)
    };
}
#endregion

        #endregion

        #region Helper Methods
        private DrPrescription GetPrescription(int id)
        {
            using var con = GetConn();
            con.Open();

            using var cmd = new MySqlCommand(
                "SELECT * FROM DrPrescription WHERE DrPrescriptionId=@Id;", con);
            cmd.Parameters.AddWithValue("@Id", id);

            using var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                return new DrPrescription
                {
                    DrPrescriptionId = dr.GetInt32("DrPrescriptionId"),
                    PatientId = dr.GetInt32("PatientId"),
                    EntryType = dr.GetString("EntryType"),
                    EntryNumber = dr.GetInt32("EntryNumber"),
                    EntryPeriod = dr.GetString("EntryPeriod"),
                    DeseaseId = dr.GetInt32("DeseaseId"),
                    Height = dr.GetDecimal("Height"),
                    Weight = dr.GetDecimal("Weight"),
                    BMI = dr.GetDecimal("BMI"),
                    TempInCelcius = dr.GetDecimal("TempInCelcius"),
                    BP = dr.GetString("BP"),
                    SPO2 = dr.GetString("SPO2"),
                    PulseRate = dr.GetDecimal("PulseRate"),
                    NextVisitDate = dr["NextVisitDate"] != DBNull.Value ? dr.GetDateTime("NextVisitDate") : null,
                    PrescriptionDate = dr.GetDateTime("PrescriptionDate")
                };
            }
            return null;
        }

        private List<DrPrescriptionDetails> GetPrescriptionDetails(int id)
        {
            var list = new List<DrPrescriptionDetails>();
            using var con = GetConn();
            con.Open();

            using var cmd = new MySqlCommand(
                @"SELECT d.*, p.ProductName, dp.DoseDescription 
                  FROM DrPrescriptionDetails d
                  LEFT JOIN ProductMaster p ON d.ProductId = p.ProductCode
                  LEFT JOIN DosePattern dp ON d.DosePatternId = dp.DosePatternId
                  WHERE DrPrescriptionId=@Id;", con);
            cmd.Parameters.AddWithValue("@Id", id);

            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new DrPrescriptionDetails
                {
                    DrPrescriptionDetailsId = dr.GetInt32("DrPrescriptionDetailsId"),
                    ProductId = dr.GetInt32("ProductId"),
                    ProductName = dr["ProductName"] != DBNull.Value ? dr.GetString("ProductName") : null,
                    DosePatternId = dr.GetInt32("DosePatternId"),
                    DosePattern = dr["DoseDescription"] != DBNull.Value ? dr.GetString("DoseDescription") : null,
                    DoseUsage = dr["DoseUsage"] != DBNull.Value ? dr.GetString("DoseUsage") : null,
                    DoseDays = dr.GetInt32("DoseDays"),
                    Qty = dr.GetInt32("Qty")
                });
            }
            return list;
        }

        private List<DrPrescriptionClinical> GetClinicalDetails(int id)
        {
            var list = new List<DrPrescriptionClinical>();
            using var con = GetConn();
            con.Open();

            using var cmd = new MySqlCommand("SELECT * FROM DrPrescriptionClinical WHERE DrPrescriptionId=@Id;", con);
            cmd.Parameters.AddWithValue("@Id", id);

            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new DrPrescriptionClinical
                {
                    DrPrescriptionClinicalId = dr.GetInt32("DrPrescriptionClinicalId"),
                    DrPrescriptionId = dr.GetInt32("DrPrescriptionId"),
                    ClinicalNote = dr["ClinicalNote"] != DBNull.Value ? dr.GetString("ClinicalNote") : null,
                    Result = dr["Result"] != DBNull.Value ? dr.GetString("Result") : null
                });
            }
            return list;
        }

        private List<PatientsMaster> LoadPatients()
        {
            var list = new List<PatientsMaster>();
            using var con = GetConn();
            con.Open();

            using var cmd = new MySqlCommand("CALL sp_GetPatients('');", con);
            using var dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                list.Add(new PatientsMaster
                {
                    PatientId = dr.GetInt32("PatientId"),
                    PatientName = dr.GetString("PatientName"),
                    UHIDNo = dr.GetString("UHIDNo"),
                    Address = dr.GetString("Address"),
                    MobileNo = dr.GetString("MobileNo"),
                    Sex = dr.GetString("Sex"),
                    BloodGroup = dr.GetString("BloodGroup")
                });
            }
            return list;
        }

        private List<DiseaseMaster> LoadDiseases()
        {
            var list = new List<DiseaseMaster>();
            using var con = GetConn();
            con.Open();

            using var cmd = new MySqlCommand("SELECT * FROM diseasemaster;", con);
            using var dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                list.Add(new DiseaseMaster
                {
                    DeseaseId = dr.GetInt32("DeseaseId"),
                    DeseaseDetails = dr.GetString("Desease")
                });
            }
            return list;
        }

        private List<DosePattern> LoadDosePatterns()
        {
            var list = new List<DosePattern>();
            using var con = GetConn();
            con.Open();

            using var cmd = new MySqlCommand("SELECT * FROM dosepattern;", con);
            using var dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                list.Add(new DosePattern
                {
                    DosePatternId = dr.GetInt32("DosePatternId"),
                    DoseDescription = dr.GetString("DoseDescription")
                });
            }
            return list;
        }

        private List<ProductMaster> LoadMedicines()
        {
            var list = new List<ProductMaster>();
            using var con = GetConn();
            con.Open();

            using var cmd = new MySqlCommand("CALL sp_GetMedicines();", con);
            using var dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                list.Add(new ProductMaster
                {
                    ProductCode = dr.GetInt32("ProductCode"),
                    ProductName = dr.GetString("ProductName")
                });
            }
            return list;
        }
        #endregion
    }
}
