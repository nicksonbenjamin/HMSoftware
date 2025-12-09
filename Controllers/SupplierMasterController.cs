using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ClinicApp.Models;
using ClinicApp.ViewModels;
using ClinicApp.Data;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Logging;

namespace ClinicApp.Controllers
{
    public class SupplierMasterController : Controller
    {
        private readonly ILogger<SupplierMasterController> _logger;
        private readonly ClinicAppMySqlDbContext _db;
        private readonly AutoMapper.IMapper _mapper;
        private readonly SupplierMasterViewModel _supplierVM;

        public SupplierMasterController(
            ILogger<SupplierMasterController> logger,
            ClinicAppMySqlDbContext db,
            SupplierMasterViewModel supplierVM,
            AutoMapper.IMapper mapper)
        {
            _logger = logger;
            _db = db;
            _mapper = mapper;
            _supplierVM = supplierVM;
            _supplierVM.Remarks = string.Empty;
        }

        // GET: SupplierMaster
        public IActionResult Index()
        {
            List<SupplierMaster> suppliers = new List<SupplierMaster>();

            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand("SELECT * FROM SupplierMaster", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                suppliers.Add(new SupplierMaster
                {
                    SupplierId = reader.GetInt32(reader.GetOrdinal("SupplierId")),
                    SupplierName = reader.GetString(reader.GetOrdinal("SupplierName")),
                    ContactPerson = reader.IsDBNull(reader.GetOrdinal("ContactPerson")) ? null : reader.GetString(reader.GetOrdinal("ContactPerson")),
                    MobileNo = reader.IsDBNull(reader.GetOrdinal("MobileNo")) ? null : reader.GetString(reader.GetOrdinal("MobileNo")),
                    Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                    Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString(reader.GetOrdinal("Address")),
                    GSTNo = reader.IsDBNull(reader.GetOrdinal("GSTNo")) ? null : reader.GetString(reader.GetOrdinal("GSTNo")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                });
            }

            List<SupplierMasterViewModel> supplierVMList = new List<SupplierMasterViewModel>();
            foreach (var supplier in suppliers)
            {
                supplierVMList.Add(_mapper.Map<SupplierMasterViewModel>(supplier));
            }

            return View(supplierVMList);
        }

        // GET: SupplierMaster/Details/5
        public IActionResult Details(int id)
        {
            if (id <= 0) return NotFound();

            SupplierMaster supplier = null;
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand("SELECT * FROM SupplierMaster WHERE SupplierId=@Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                supplier = new SupplierMaster
                {
                    SupplierId = reader.GetInt32(reader.GetOrdinal("SupplierId")),
                    SupplierName = reader.GetString(reader.GetOrdinal("SupplierName")),
                    ContactPerson = reader.IsDBNull(reader.GetOrdinal("ContactPerson")) ? null : reader.GetString(reader.GetOrdinal("ContactPerson")),
                    MobileNo = reader.IsDBNull(reader.GetOrdinal("MobileNo")) ? null : reader.GetString(reader.GetOrdinal("MobileNo")),
                    Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                    Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString(reader.GetOrdinal("Address")),
                    GSTNo = reader.IsDBNull(reader.GetOrdinal("GSTNo")) ? null : reader.GetString(reader.GetOrdinal("GSTNo")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                };
            }

            var supplierVM = _mapper.Map<SupplierMasterViewModel>(supplier);
            return View(supplierVM);
        }

        // GET: SupplierMaster/Create
        public IActionResult Create()
        {
            return View(_supplierVM);
        }

        // POST: SupplierMaster/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SupplierMasterViewModel supplierVM)
        {
            var supplier = _mapper.Map<SupplierMaster>(supplierVM);

            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand(@"
                INSERT INTO SupplierMaster
                (SupplierName, ContactPerson, MobileNo, Email, Address, GSTNo, IsActive)
                VALUES
                (@SupplierName, @ContactPerson, @MobileNo, @Email, @Address, @GSTNo, @IsActive)", conn);

            cmd.Parameters.AddWithValue("@SupplierName", supplier.SupplierName);
            cmd.Parameters.AddWithValue("@ContactPerson", supplier.ContactPerson);
            cmd.Parameters.AddWithValue("@MobileNo", supplier.MobileNo);
            cmd.Parameters.AddWithValue("@Email", supplier.Email);
            cmd.Parameters.AddWithValue("@Address", supplier.Address);
            cmd.Parameters.AddWithValue("@GSTNo", supplier.GSTNo);
            cmd.Parameters.AddWithValue("@IsActive", supplier.IsActive);

            cmd.ExecuteNonQuery();
            return RedirectToAction(nameof(Index));
        }

        // GET: SupplierMaster/Edit/5
        public IActionResult Edit(int id)
        {
            if (id <= 0) return NotFound();

            SupplierMaster supplier = null;
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand("SELECT * FROM SupplierMaster WHERE SupplierId=@Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                supplier = new SupplierMaster
                {
                    SupplierId = reader.GetInt32(reader.GetOrdinal("SupplierId")),
                    SupplierName = reader.GetString(reader.GetOrdinal("SupplierName")),
                    ContactPerson = reader.IsDBNull(reader.GetOrdinal("ContactPerson")) ? null : reader.GetString(reader.GetOrdinal("ContactPerson")),
                    MobileNo = reader.IsDBNull(reader.GetOrdinal("MobileNo")) ? null : reader.GetString(reader.GetOrdinal("MobileNo")),
                    Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                    Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString(reader.GetOrdinal("Address")),
                    GSTNo = reader.IsDBNull(reader.GetOrdinal("GSTNo")) ? null : reader.GetString(reader.GetOrdinal("GSTNo")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                };
            }

            var supplierVM = _mapper.Map<SupplierMasterViewModel>(supplier);
            return View(supplierVM);
        }

        // POST: SupplierMaster/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, SupplierMasterViewModel supplierVM)
        {
            if (id != supplierVM.SupplierId) return NotFound();

            var supplier = _mapper.Map<SupplierMaster>(supplierVM);

            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand(@"
                UPDATE SupplierMaster SET 
                    SupplierName=@SupplierName,
                    ContactPerson=@ContactPerson,
                    MobileNo=@MobileNo,
                    Email=@Email,
                    Address=@Address,
                    GSTNo=@GSTNo,
                    IsActive=@IsActive
                WHERE SupplierId=@Id", conn);

            cmd.Parameters.AddWithValue("@Id", supplier.SupplierId);
            cmd.Parameters.AddWithValue("@SupplierName", supplier.SupplierName);
            cmd.Parameters.AddWithValue("@ContactPerson", supplier.ContactPerson);
            cmd.Parameters.AddWithValue("@MobileNo", supplier.MobileNo);
            cmd.Parameters.AddWithValue("@Email", supplier.Email);
            cmd.Parameters.AddWithValue("@Address", supplier.Address);
            cmd.Parameters.AddWithValue("@GSTNo", supplier.GSTNo);
            cmd.Parameters.AddWithValue("@IsActive", supplier.IsActive);

            cmd.ExecuteNonQuery();
            return RedirectToAction(nameof(Index));
        }

        // GET: SupplierMaster/Delete/5
        public IActionResult Delete(int id)
        {
            if (id <= 0) return NotFound();

            SupplierMaster supplier = null;
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand("SELECT * FROM SupplierMaster WHERE SupplierId=@Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                supplier = new SupplierMaster
                {
                    SupplierId = reader.GetInt32(reader.GetOrdinal("SupplierId")),
                    SupplierName = reader.GetString(reader.GetOrdinal("SupplierName")),
                    ContactPerson = reader.IsDBNull(reader.GetOrdinal("ContactPerson")) ? null : reader.GetString(reader.GetOrdinal("ContactPerson")),
                    MobileNo = reader.IsDBNull(reader.GetOrdinal("MobileNo")) ? null : reader.GetString(reader.GetOrdinal("MobileNo")),
                    Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                    Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString(reader.GetOrdinal("Address")),
                    GSTNo = reader.IsDBNull(reader.GetOrdinal("GSTNo")) ? null : reader.GetString(reader.GetOrdinal("GSTNo")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                };
            }

            var supplierVM = _mapper.Map<SupplierMasterViewModel>(supplier);
            return View(supplierVM);
        }

        // POST: SupplierMaster/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand("DELETE FROM SupplierMaster WHERE SupplierId=@Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();
            return RedirectToAction(nameof(Index));
        }
    }
}
