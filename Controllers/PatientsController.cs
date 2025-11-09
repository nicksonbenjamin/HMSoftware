using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClinicApp.ViewModels;
using ClinicApp.Data;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Logging;
using ClinicApp.Models; 

namespace ClinicApp.Controllers
{
    public class PatientsController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        public PatientViewModel _patientVM;
        private readonly ClinicAppMySqlDbContext _db;
        private readonly AutoMapper.IMapper _mapper;
        public PatientsController(ILogger<AuthController> logger, ClinicAppMySqlDbContext db, PatientViewModel patientVM, AutoMapper.IMapper mapper)
        {
            _logger = logger;
            _patientVM = patientVM;
            _db = db;
            _patientVM.ErrorMessage = string.Empty;
            _mapper = mapper;
        }


        // GET: Visits
        public IActionResult Index()
        {
            List<Patient> patients = new List<Patient>();
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand("SELECT PatientId, PatientName, MobileNo, Age, Sex FROM Patients_New", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                patients.Add(new Patient
                {
                    PatientId = reader.GetGuid("PatientId"),
                    PatientName = reader.GetString("PatientName"),
                    MobileNo = reader.GetString("MobileNo"),
                    Age = reader.GetInt32("Age"),
                    Sex = reader.GetString("Sex")
                });
            }
            List<PatientViewModel> patientVMList = new List<PatientViewModel>();
            foreach (var patient in patients)
            {
                var patientVM = _mapper.Map<PatientViewModel>(patient);
                patientVMList.Add(patientVM);
            }
            return View(patientVMList);
        }

        // GET: Visits/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Patient patient = null;
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand("SELECT * FROM Patients_New Where PatientId=@PatientId", conn);
            cmd.Parameters.AddWithValue("@PatientId", id);
            using var reader = cmd.ExecuteReader();
            if(reader.Read())
            {
                patient = new Patient
                {
                    PatientId = reader.GetGuid("PatientId"),
                    UHIDType = reader.GetString("UHIDType"),
                    UHIDNo = reader.GetString("UHIDNo"),
                    RegistrationDate = reader.GetDateTime("RegistrationDate"),
                    RegistrationTime = reader.GetTimeSpan("RegistrationTime"),
                    Sex = reader.GetString("Sex"),
                    PatientTitle = reader.GetString("PatientTitle"),
                    PatientName = reader.GetString("PatientName"),
                    DOB = reader.GetDateTime("DOB"),
                    Age = reader.GetInt32("Age"),
                    GuardianTitle = reader.GetString("GuardianTitle"),
                    Guardian = reader.GetString("Guardian"),
                    Address = reader.GetString("Address"),
                    Place = reader.GetString("Place"),
                    District = reader.GetString("District"),
                    GSTState = reader.GetString("GSTState"),
                    Country = reader.GetString("Country"),
                    PinCode = reader.GetString("PinCode"),
                    PatientAadhar = reader.GetString("PatientAadhar"),
                    GuardianAadhar = reader.GetString("GuardianAadhar"),
                    MobileNo = reader.GetString("MobileNo"),
                    Email = reader.GetString("Email"),
                    Occupation = reader.GetString("Occupation"),
                    MaritalStatus = reader.GetString("MaritalStatus"),
                    BloodGroup = reader.GetString("BloodGroup"),
                    AllergicTo = reader.GetString("AllergicTo"),
                    //Photo = (byte[])(reader["Photo"] is DBNull ? null : reader["Photo"]),
                    ConsultantDoctor = reader.GetString("ConsultantDoctor"),
                    RefDoctor = reader.GetString("RefDoctor"),
                    PaymentTerms = reader.GetString("PaymentTerms"),
                    RegistrationType = reader.GetString("RegistrationType"),
                    CompOrInsOrCamp = reader.GetString("CompOrInsOrCamp"),
                    PatientCondition = reader.GetString("PatientCondition"),
                    ReferenceOrPicmeNo = reader.GetString("ReferenceOrPicmeNo"),
                    IsActive = reader.GetBoolean("IsActive"),
                    CreatedBy = reader.GetString("CreatedBy"),
                    UpdatedBy = reader.GetString("UpdatedBy"),
                    CreatedDate = reader.GetDateTime("CreatedDate"),
                    UpdatedDate = reader.GetDateTime("UpdatedDate")
                };
            }
            PatientViewModel patientVM = new PatientViewModel();
            patientVM = _mapper.Map<PatientViewModel>(patient);
            return View(patientVM);
        }

        // GET: Visits/Create
        public IActionResult Create()
        {

            //ViewData["AttendingStaffID"] = new SelectList(_context.AttendingStaff, "StaffID", "StaffID");
            //ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "FirstName");
            return View();
        }

        // POST: Visits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PatientViewModel patient)
        {
            patient.PatientId = Guid.NewGuid().ToString();
            patient.CreatedDate = DateTime.Now;

            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT INTO Patients_New 
            (PatientId, UHIDType, UHIDNo, RegistrationDate, RegistrationTime, Sex, PatientTitle, PatientName, DOB, Age,
             GuardianTitle, Guardian, Address, Place, District, GSTState, Country, PinCode, PatientAadhar, GuardianAadhar,
             MobileNo, Email, Photo, Occupation, MaritalStatus, BloodGroup, AllergicTo, ConsultantDoctor, RefDoctor,
             PaymentTerms, RegistrationType, CompOrInsOrCamp, PatientCondition, ReferenceOrPicmeNo, IsActive,
             CreatedBy, CreatedDate)
             VALUES (@PatientId, @UHIDType, @UHIDNo, @RegistrationDate, @RegistrationTime, @Sex, @PatientTitle, @PatientName, @DOB, @Age,
             @GuardianTitle, @Guardian, @Address, @Place, @District, @GSTState, @Country, @PinCode, @PatientAadhar, @GuardianAadhar,
             @MobileNo, @Email, @Photo, @Occupation, @MaritalStatus, @BloodGroup, @AllergicTo, @ConsultantDoctor, @RefDoctor,
             @PaymentTerms, @RegistrationType, @CompOrInsOrCamp, @PatientCondition, @ReferenceOrPicmeNo, @IsActive,
             @CreatedBy, @CreatedDate)";

            cmd.Parameters.AddWithValue("@PatientId", patient.PatientId);
            cmd.Parameters.AddWithValue("@UHIDType", patient.UHIDType);
            cmd.Parameters.AddWithValue("@UHIDNo", patient.UHIDNo);
            cmd.Parameters.AddWithValue("@RegistrationDate", patient.RegistrationDate);
            cmd.Parameters.AddWithValue("@RegistrationTime", patient.RegistrationTime);
            cmd.Parameters.AddWithValue("@Sex", patient.Sex);
            cmd.Parameters.AddWithValue("@PatientTitle", patient.PatientTitle);
            cmd.Parameters.AddWithValue("@PatientName", patient.PatientName);
            cmd.Parameters.AddWithValue("@DOB", patient.DOB);
            cmd.Parameters.AddWithValue("@Age", patient.Age);
            cmd.Parameters.AddWithValue("@GuardianTitle", patient.GuardianTitle);
            cmd.Parameters.AddWithValue("@Guardian", patient.Guardian);
            cmd.Parameters.AddWithValue("@Address", patient.Address);
            cmd.Parameters.AddWithValue("@Place", patient.Place);
            cmd.Parameters.AddWithValue("@District", patient.District);
            cmd.Parameters.AddWithValue("@GSTState", patient.GSTState);
            cmd.Parameters.AddWithValue("@Country", patient.Country);
            cmd.Parameters.AddWithValue("@PinCode", patient.PinCode);
            cmd.Parameters.AddWithValue("@PatientAadhar", patient.PatientAadhar);
            cmd.Parameters.AddWithValue("@GuardianAadhar", patient.GuardianAadhar);
            cmd.Parameters.AddWithValue("@MobileNo", patient.MobileNo);
            cmd.Parameters.AddWithValue("@Email", patient.Email ?? "");
            cmd.Parameters.AddWithValue("@Photo", patient.Photo ?? new byte[0]);
            cmd.Parameters.AddWithValue("@Occupation", patient.Occupation);
            cmd.Parameters.AddWithValue("@MaritalStatus", patient.MaritalStatus);
            cmd.Parameters.AddWithValue("@BloodGroup", patient.BloodGroup);
            cmd.Parameters.AddWithValue("@AllergicTo", patient.AllergicTo);
            cmd.Parameters.AddWithValue("@ConsultantDoctor", patient.ConsultantDoctor);
            cmd.Parameters.AddWithValue("@RefDoctor", patient.RefDoctor);
            cmd.Parameters.AddWithValue("@PaymentTerms", patient.PaymentTerms);
            cmd.Parameters.AddWithValue("@RegistrationType", patient.RegistrationType);
            cmd.Parameters.AddWithValue("@CompOrInsOrCamp", patient.CompOrInsOrCamp);
            cmd.Parameters.AddWithValue("@PatientCondition", patient.PatientCondition);
            cmd.Parameters.AddWithValue("@ReferenceOrPicmeNo", patient.ReferenceOrPicmeNo);
            cmd.Parameters.AddWithValue("@IsActive", patient.IsActive);
            cmd.Parameters.AddWithValue("@CreatedBy", patient.CreatedBy);
            cmd.Parameters.AddWithValue("@CreatedDate", patient.CreatedDate);
            if (cmd.ExecuteNonQuery() > 0)
            {
                return RedirectToAction("Index");
            }
            // ViewData["AttendingStaffID"] = new SelectList(_context.AttendingStaff, "StaffID", "StaffID", visit.AttendingStaffID);
            // ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "FirstName", visit.PatientID);
            else
            {
                patient.ErrorMessage = "Error occurred while creating patient record.";
                return View(patient);
            }
            
        }

        // GET: Visits/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Patient patient = null;
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand("SELECT * FROM Patients_New Where PatientId=@PatientId", conn);
            cmd.Parameters.AddWithValue("@PatientId", id);
            using var reader = cmd.ExecuteReader();
            if(reader.Read())
            {
                patient = new Patient
                {
                    PatientId = reader.GetGuid("PatientId"),
                    UHIDType = reader.GetString("UHIDType"),
                    UHIDNo = reader.GetString("UHIDNo"),
                    RegistrationDate = reader.GetDateTime("RegistrationDate"),
                    RegistrationTime = reader.GetTimeSpan("RegistrationTime"),
                    Sex = reader.GetString("Sex"),
                    PatientTitle = reader.GetString("PatientTitle"),
                    PatientName = reader.GetString("PatientName"),
                    DOB = reader.GetDateTime("DOB"),
                    Age = reader.GetInt32("Age"),
                    GuardianTitle = reader.GetString("GuardianTitle"),
                    Guardian = reader.GetString("Guardian"),
                    Address = reader.GetString("Address"),
                    Place = reader.GetString("Place"),
                    District = reader.GetString("District"),
                    GSTState = reader.GetString("GSTState"),
                    Country = reader.GetString("Country"),
                    PinCode = reader.GetString("PinCode"),
                    PatientAadhar = reader.GetString("PatientAadhar"),
                    GuardianAadhar = reader.GetString("GuardianAadhar"),
                    MobileNo = reader.GetString("MobileNo"),
                    Email = reader.GetString("Email"),
                    Occupation = reader.GetString("Occupation"),
                    MaritalStatus = reader.GetString("MaritalStatus"),
                    BloodGroup = reader.GetString("BloodGroup"),
                    AllergicTo = reader.GetString("AllergicTo"),
                    //Photo = (byte[])(reader["Photo"] is DBNull ? null : reader["Photo"]),
                    ConsultantDoctor = reader.GetString("ConsultantDoctor"),
                    RefDoctor = reader.GetString("RefDoctor"),
                    PaymentTerms = reader.GetString("PaymentTerms"),
                    RegistrationType = reader.GetString("RegistrationType"),
                    CompOrInsOrCamp = reader.GetString("CompOrInsOrCamp"),
                    PatientCondition = reader.GetString("PatientCondition"),
                    ReferenceOrPicmeNo = reader.GetString("ReferenceOrPicmeNo"),
                    IsActive = reader.GetBoolean("IsActive"),
                    CreatedBy = reader.GetString("CreatedBy"),
                    UpdatedBy = reader.GetString("UpdatedBy"),
                    CreatedDate = reader.GetDateTime("CreatedDate"),
                    UpdatedDate = reader.GetDateTime("UpdatedDate")
                };
            }
            PatientViewModel patientVM = new PatientViewModel();
            patientVM = _mapper.Map<PatientViewModel>(patient);
            return View(patientVM);

            // ViewData["AttendingStaffID"] = new SelectList(_context.AttendingStaff, "StaffID", "StaffID", visit.AttendingStaffID);
            // ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "FirstName", visit.PatientID);
        }

        // POST: Visits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, PatientViewModel patient)
        {
            if (!Guid.Equals(id, patient.PatientId))
            {
                return NotFound();
            }
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand(@"UPDATE ApplicationUser SET 
            PatientId=@PatientId, UHIDType=@UHIDType, UHIDNo=@UHIDNo, RegistrationDate=@RegistrationDate, RegistrationTime=@RegistrationTime, Sex=@Sex, PatientTitle=@PatientTitle, PatientName=@PatientName, DOB=@DOB, Age=@Age,
            GuardianTitle=@GuardianTitle, Guardian=@Guardian, Address=@Address, Place=@Place, District=@District, GSTState=@GSTState, Country=@Country, PinCode=@PinCode, PatientAadhar=@PatientAadhar, GuardianAadhar=@GuardianAadhar,
            MobileNo=@MobileNo, Email=@Email, Photo=@Photo, Occupation=@Occupation, MaritalStatus=@MaritalStatus, BloodGroup=@BloodGroup, AllergicTo=@AllergicTo, ConsultantDoctor=@ConsultantDoctor, RefDoctor=@RefDoctor,
            PaymentTerms=@PaymentTerms, RegistrationType=@RegistrationType, CompOrInsOrCamp=@CompOrInsOrCamp, PatientCondition=@PatientCondition, ReferenceOrPicmeNo=@ReferenceOrPicmeNo, IsActive=@IsActive,
            UpdatedBy=@UpdatedBy, UpdatedDate=@UpdatedDate 
            WHERE UserId=@UserId", conn);
            cmd.Parameters.AddWithValue("@PatientId", patient.PatientId);
            cmd.Parameters.AddWithValue("@UHIDType", patient.UHIDType);
            cmd.Parameters.AddWithValue("@UHIDNo", patient.UHIDNo);
            cmd.Parameters.AddWithValue("@RegistrationDate", patient.RegistrationDate);
            cmd.Parameters.AddWithValue("@RegistrationTime", patient.RegistrationTime);
            cmd.Parameters.AddWithValue("@Sex", patient.Sex);
            cmd.Parameters.AddWithValue("@PatientTitle", patient.PatientTitle);
            cmd.Parameters.AddWithValue("@PatientName", patient.PatientName);
            cmd.Parameters.AddWithValue("@DOB", patient.DOB);
            cmd.Parameters.AddWithValue("@Age", patient.Age);
            cmd.Parameters.AddWithValue("@GuardianTitle", patient.GuardianTitle);
            cmd.Parameters.AddWithValue("@Guardian", patient.Guardian);
            cmd.Parameters.AddWithValue("@Address", patient.Address);
            cmd.Parameters.AddWithValue("@Place", patient.Place);
            cmd.Parameters.AddWithValue("@District", patient.District);
            cmd.Parameters.AddWithValue("@GSTState", patient.GSTState);
            cmd.Parameters.AddWithValue("@Country", patient.Country);
            cmd.Parameters.AddWithValue("@PinCode", patient.PinCode);
            cmd.Parameters.AddWithValue("@PatientAadhar", patient.PatientAadhar);
            cmd.Parameters.AddWithValue("@GuardianAadhar", patient.GuardianAadhar);
            cmd.Parameters.AddWithValue("@MobileNo", patient.MobileNo);
            cmd.Parameters.AddWithValue("@Email", patient.Email ?? "");
            cmd.Parameters.AddWithValue("@Photo", patient.Photo ?? new byte[0]);
            cmd.Parameters.AddWithValue("@Occupation", patient.Occupation);
            cmd.Parameters.AddWithValue("@MaritalStatus", patient.MaritalStatus);
            cmd.Parameters.AddWithValue("@BloodGroup", patient.BloodGroup);
            cmd.Parameters.AddWithValue("@AllergicTo", patient.AllergicTo);
            cmd.Parameters.AddWithValue("@ConsultantDoctor", patient.ConsultantDoctor);
            cmd.Parameters.AddWithValue("@RefDoctor", patient.RefDoctor);
            cmd.Parameters.AddWithValue("@PaymentTerms", patient.PaymentTerms);
            cmd.Parameters.AddWithValue("@RegistrationType", patient.RegistrationType);
            cmd.Parameters.AddWithValue("@CompOrInsOrCamp", patient.CompOrInsOrCamp);
            cmd.Parameters.AddWithValue("@PatientCondition", patient.PatientCondition);
            cmd.Parameters.AddWithValue("@ReferenceOrPicmeNo", patient.ReferenceOrPicmeNo);
            cmd.Parameters.AddWithValue("@IsActive", patient.IsActive);
            cmd.Parameters.AddWithValue("@UpdatedBy", "Admin");
            cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
            if (cmd.ExecuteNonQuery() > 0)
            {
                return RedirectToAction("Index");
            }
            // ViewData["AttendingStaffID"] = new SelectList(_context.AttendingStaff, "StaffID", "StaffID", visit.AttendingStaffID);
            // ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "FirstName", visit.PatientID);
            else
            {
                patient.ErrorMessage = "Error occurred while creating patient record.";
                return View(patient);
            }
        }

        // GET: Visits/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            return View(id);
        }

        // POST: Visits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand("DELETE FROM Patients_New WHERE PatientId=@PatientId", conn);
            cmd.Parameters.AddWithValue("@PatientId", id);
            if (cmd.ExecuteNonQuery() > 0)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(id);
        }
    }
}
