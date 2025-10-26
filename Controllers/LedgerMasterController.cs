using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClinicApp.Models;
using ClinicApp.ViewModels;
using ClinicApp.Data;
using MySql.Data.MySqlClient;

namespace ClinicApp.Controllers
{
    public class LedgerMasterController : Controller
    {
        private readonly ILogger<LedgerMasterController> _logger;
        public LedgerMasterViewModel _ledgerMasterVM;
        private readonly ClinicAppMySqlDbContext _db;
        private readonly AutoMapper.IMapper _mapper;
        public LedgerMasterController(ILogger<LedgerMasterController> logger, ClinicAppMySqlDbContext db, LedgerMasterViewModel ledgerMasterVM, AutoMapper.IMapper mapper)
        {
            _logger = logger;
            _ledgerMasterVM = ledgerMasterVM;
            _db = db;
            _ledgerMasterVM.ErrorMessage = string.Empty;
            _mapper = mapper;
        }

        // GET: LedgerMaster
        public IActionResult Index()
        {
            List<LedgerMaster> Ledgers = new List<LedgerMaster>();
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand(@"SELECT * FROM LedgerMaster", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Ledgers.Add(new LedgerMaster
                {
                    Id = reader.GetInt32("Id"),
                    LedgerName = reader.GetString("ledger_name"),
                    LedgerType = reader.GetString("ledger_type"),
                    Address = reader.GetString("address"),
                    Place = reader.GetString("place"),
                    GSTIN = reader.GetString("gstin"),
                    GSTState = reader.GetString("gst_state"),
                    CreditDays = reader.GetInt32("credit_days"),
                    ContactPerson = reader.GetString("contact_person"),
                    PhoneNo = reader.GetString("phone_no"),
                    MobileNo = reader.GetString("mobile_no"),
                    Email = reader.GetString("email"),
                    DrugLicenseNo = reader.GetString("drug_license_no"),
                    Remarks = reader.GetString("remarks"),
                    IsActive = reader.GetBoolean("is_active"),
                    CreatedAt = reader.GetDateTime("created_at"),
                    UpdatedAt= reader.GetDateTime("updated_at"),
                    // RoleName = reader.IsDBNull(reader.GetOrdinal("RoleName")) ? "" : reader.GetString("RoleName")
                });
            }
            List<LedgerMasterViewModel> ledgerMasterVMList = new List<LedgerMasterViewModel>();
            foreach (var ledger in Ledgers)
            {
                LedgerMasterViewModel ledgerMasterViewModel = _mapper.Map<LedgerMasterViewModel>(ledger);
                ledgerMasterVMList.Add(ledgerMasterViewModel);
            }

            return View(ledgerMasterVMList);
        }

        // GET: LedgerMaster/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            LedgerMaster ledger = null;
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand(@"
            SELECT * FROM LedgerMaster WHERE id=@Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                ledger = new LedgerMaster
                {
                    Id = reader.GetInt32("Id"),
                    LedgerName = reader.GetString("ledger_name"),
                    LedgerType = reader.GetString("ledger_type"),
                    Address = reader.GetString("address"),
                    Place = reader.GetString("place"),
                    GSTIN = reader.GetString("gstin"),
                    GSTState = reader.GetString("gst_state"),
                    CreditDays = reader.GetInt32("credit_days"),
                    ContactPerson = reader.GetString("contact_person"),
                    PhoneNo = reader.GetString("phone_no"),
                    MobileNo = reader.GetString("mobile_no"),
                    Email = reader.GetString("email"),
                    DrugLicenseNo = reader.GetString("drug_license_no"),
                    Remarks = reader.GetString("remarks"),
                    IsActive = reader.GetBoolean("is_active"),
                    CreatedAt = reader.GetDateTime("created_at"),
                    UpdatedAt= reader.GetDateTime("updated_at")
                };
            }
            LedgerMasterViewModel ledgerMaterVM = _mapper.Map<LedgerMasterViewModel>(ledger);
            return View(ledgerMaterVM);
        }

        // GET: LedgerMaster/Create
        public IActionResult Create()
        {
            // List<SelectListItem> roles = new List<SelectListItem>();
            // using var conn = _db.GetConnection();
            // conn.Open();
            // var cmd = new MySqlCommand("SELECT RoleId, RoleName FROM Roles", conn);
            // using var reader = cmd.ExecuteReader();
            // while (reader.Read())
            // {
            //     roles.Add(new SelectListItem
            //     {
            //         Value = reader.GetGuid("RoleId").ToString(),
            //         Text = reader.GetString("RoleName")
            //     });
            // }
            // _appUserVM.Roles = roles;
            // //ViewData["RoleID"] = new SelectList(_context.Roles, "RoleId", "RoleId");
            // return View(_appUserVM);
            return View();
        }

        // POST: LedgerMaster/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(LedgerMasterViewModel ledgerMasterVM)
        {
            LedgerMaster ledgerMaster = _mapper.Map<LedgerMaster>(ledgerMasterVM);
            // if (!ModelState.IsValid)
            // {
            //     List<SelectListItem> roles = new List<SelectListItem>();
            //     using var connection = _db.GetConnection();
            //     connection.Open();
            //     var command = new MySqlCommand("SELECT RoleId, RoleName FROM Roles", connection);
            //     using var reader = command.ExecuteReader();
            //     while (reader.Read())
            //     {
            //         roles.Add(new SelectListItem
            //         {
            //             Value = reader.GetGuid("RoleId").ToString(),
            //             Text = reader.GetString("RoleName")
            //         });
            //     }
            //     _appUserVM.Roles = roles;
            //     return View(_appUserVM);
            // }
            // else
            // {

           // ledgerMaster.Id = Guid.NewGuid();
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand(@"INSERT INTO LedgerMaster 
                (ledger_type, ledger_name, address, place, gstin, gst_state, credit_days,contact_person,phone_no,
                mobile_no,email,drug_license_no,remarks,is_active,created_at,updated_at) 
                VALUES (@ledger_type, @ledger_name, @address, @place, @gstin, @gst_state, @credit_days,@contact_person,@phone_no,
                @mobile_no,@email,@drug_license_no,@remarks,@is_active,@created_at,@updated_at)", conn);
            
            cmd.Parameters.AddWithValue("@ledger_type", ledgerMaster.LedgerType);
            cmd.Parameters.AddWithValue("@ledger_name", ledgerMaster.LedgerName);
            cmd.Parameters.AddWithValue("@address", ledgerMaster.Address);
            cmd.Parameters.AddWithValue("@place", ledgerMaster.Place);
            cmd.Parameters.AddWithValue("@gstin", ledgerMaster.GSTIN);
            cmd.Parameters.AddWithValue("@gst_state", ledgerMaster.GSTState);

            cmd.Parameters.AddWithValue("@credit_days", ledgerMaster.CreditDays);
            cmd.Parameters.AddWithValue("@contact_person", ledgerMaster.ContactPerson);
             cmd.Parameters.AddWithValue("@phone_no", ledgerMaster.PhoneNo);
            cmd.Parameters.AddWithValue("@mobile_no", ledgerMaster.MobileNo);
            cmd.Parameters.AddWithValue("@email", ledgerMaster.Email);

            cmd.Parameters.AddWithValue("@drug_license_no", ledgerMaster.DrugLicenseNo);
            cmd.Parameters.AddWithValue("@remarks", ledgerMaster.Remarks);
            cmd.Parameters.AddWithValue("@is_active", ledgerMaster.IsActive);
            cmd.Parameters.AddWithValue("@created_at", System.DateTime.Now);
            cmd.Parameters.AddWithValue("@updated_at", System.DateTime.Now);
            cmd.ExecuteNonQuery();
            return RedirectToAction("Index");
            //}
            //ViewData["RoleID"] = new SelectList(_context.Roles, "RoleId", "RoleId", applicationUser.RoleID);
        }

        // GET: LedgerMaster/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

         //   List<SelectListItem> Roles = new List<SelectListItem>();
            using var conn = _db.GetConnection();
            conn.Open();

            // var roleCmd = new MySqlCommand("SELECT RoleId, RoleName FROM Roles", conn);
            // using var roleReader = roleCmd.ExecuteReader();
            // while (roleReader.Read())
            // {
            //     Roles.Add(new SelectListItem
            //     {
            //         Value = Convert.ToString(roleReader.GetGuid("RoleId")),
            //         Text = roleReader.GetString("RoleName")
            //     });
            // }
            // roleReader.Close();

            LedgerMaster ledgerMaster = null;
            var cmd = new MySqlCommand("SELECT * FROM LedgerMaster WHERE Id=@Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                ledgerMaster = new LedgerMaster
                {
                    Id = reader.GetInt32("Id"),
                    LedgerName = reader.GetString("ledger_name"),
                    LedgerType = reader.GetString("ledger_type"),
                    Address = reader.GetString("address"),
                    Place = reader.GetString("place"),
                    GSTIN = reader.GetString("gstin"),
                    GSTState = reader.GetString("gst_state"),
                    CreditDays = reader.GetInt32("credit_days"),
                    ContactPerson = reader.GetString("contact_person"),
                    PhoneNo = reader.GetString("phone_no"),
                    MobileNo = reader.GetString("mobile_no"),
                    Email = reader.GetString("email"),
                    DrugLicenseNo = reader.GetString("drug_license_no"),
                    Remarks = reader.GetString("remarks"),
                    IsActive = reader.GetBoolean("is_active"),
                    CreatedAt = reader.GetDateTime("created_at"),
                    UpdatedAt = reader.GetDateTime("updated_at"),

                };
            }

            LedgerMasterViewModel ledgerMasterVM = _mapper.Map<LedgerMasterViewModel>(ledgerMaster);
          //  appUserVM.Roles = Roles;
            return View(ledgerMasterVM);
            //ViewData["RoleID"] = new SelectList(_context.Roles, "RoleId", "RoleId", applicationUser.RoleID);
        }

        // POST: LedgerMAster/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id,  LedgerMasterViewModel ledgerMasterVM)
        {
            if (!int.Equals(id, ledgerMasterVM.Id))
            {
                return NotFound();
            }
                        
            LedgerMaster ledgerMaster = _mapper.Map<LedgerMaster>(ledgerMasterVM);
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand(@"UPDATE LedgerMaster SET 
            Ledger_Type=@ledger_type, Ledger_Name=@ledger_name, Address=@address, 
            place=@place, gstin=@gstin, gst_state=@gst_state,credit_days=@credit_days,
            contact_person=@contact_person,mobile_no=@mobile_no,email=@email,
            drug_license_no=@drug_license_no,remarks=@remarks,
            is_active=@is_active,updated_at=@updated_at 
            WHERE Id=@Id", conn);

            cmd.Parameters.AddWithValue("@Id", ledgerMaster.Id);
             cmd.Parameters.AddWithValue("@ledger_type", ledgerMaster.LedgerType);
            cmd.Parameters.AddWithValue("@ledger_name", ledgerMaster.LedgerName);
            cmd.Parameters.AddWithValue("@address", ledgerMaster.Address);
            cmd.Parameters.AddWithValue("@place", ledgerMaster.Place);
            cmd.Parameters.AddWithValue("@gstin", ledgerMaster.GSTIN);
            cmd.Parameters.AddWithValue("@gst_state", ledgerMaster.GSTState);

            cmd.Parameters.AddWithValue("@credit_days", ledgerMaster.CreditDays);
            cmd.Parameters.AddWithValue("@contact_person", ledgerMaster.ContactPerson);
            cmd.Parameters.AddWithValue("@mobile_no", ledgerMaster.MobileNo);
            cmd.Parameters.AddWithValue("@email", ledgerMaster.Email);

            cmd.Parameters.AddWithValue("@drug_license_no", ledgerMaster.DrugLicenseNo);
            cmd.Parameters.AddWithValue("@remarks", ledgerMaster.Remarks);
            cmd.Parameters.AddWithValue("@is_active", ledgerMaster.IsActive);
           // cmd.Parameters.AddWithValue("@created_at", System.DateTime.Now);
            cmd.Parameters.AddWithValue("@updated_at", System.DateTime.Now);
            cmd.ExecuteNonQuery();
            // if (ModelState.IsValid)
            // {
            return RedirectToAction(nameof(Index));
            //}
            /*
            ApplicationUserViewModel appUserVM = _mapper.Map<ApplicationUserViewModel>(applicationUser);
            List<SelectListItem> Roles = new List<SelectListItem>();
            var roleCmd = new MySqlCommand("SELECT RoleId, RoleName FROM Roles", conn);
            using var roleReader = roleCmd.ExecuteReader();
            while (roleReader.Read())
            {
                Roles.Add(new SelectListItem
                {
                    Value = roleReader.GetString("RoleId"),
                    Text = roleReader.GetString("RoleName")
                });
            }
            roleReader.Close();
            appUserVM.Roles = Roles;
            //ViewData["RoleID"] = new SelectList(_context.Roles, "RoleId", "RoleId", applicationUser.RoleID);
            return View(appUserVM);
            */
        }

        // GET: LedgerMaster/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            LedgerMaster ledgerMaster = null;
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand("SELECT * FROM ledgerMaster WHERE Id=@Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                ledgerMaster = new LedgerMaster
                {
                    Id = reader.GetInt32("Id"),
                    LedgerName = reader.GetString("ledger_name"),
                    LedgerType = reader.GetString("ledger_type"),
                    Address = reader.GetString("address"),
                    Place = reader.GetString("place"),
                    GSTIN = reader.GetString("gstin"),
                    GSTState = reader.GetString("gst_state"),
                    CreditDays = reader.GetInt32("credit_days"),
                    ContactPerson = reader.GetString("contact_person"),
                    PhoneNo = reader.GetString("phone_no"),
                    MobileNo = reader.GetString("mobile_no"),
                    Email = reader.GetString("email"),
                    DrugLicenseNo = reader.GetString("drug_license_no"),
                    Remarks = reader.GetString("remarks"),
                    IsActive = reader.GetBoolean("is_active"),
                    CreatedAt = reader.GetDateTime("created_at"),
                    UpdatedAt= reader.GetDateTime("updated_at")
                };
            }
            LedgerMasterViewModel ledgerMasterVM = _mapper.Map<LedgerMasterViewModel>(ledgerMaster);
            return View(ledgerMasterVM);
        }

        // POST: LedgerMaster/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand("DELETE FROM LedgerMaster WHERE Id=@Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();
            return RedirectToAction(nameof(Index));
        }
    }
}
