using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using ClinicApp.ViewModels;
using ClinicApp.Data;
using ClinicApp.Models;
using AutoMapper;

namespace ClinicApp.Controllers
{
    public class DescriptionMasterController : Controller
    {
        private readonly ILogger<DescriptionMasterController> _logger;
        private readonly ClinicAppMySqlDbContext _db;
        private readonly IMapper _mapper;

        public DescriptionMasterController(
            ILogger<DescriptionMasterController> logger,
            ClinicAppMySqlDbContext db,
            IMapper mapper)
        {
            _logger = logger;
            _db = db;
            _mapper = mapper;
        }

        // ✅ INDEX - Get all Descriptions
        public IActionResult Index()
        {
            var list = new List<DescriptionMaster>();
            using var conn = _db.GetConnection();
            conn.Open();

            using var cmd = new MySqlCommand("GetDescriptionsMasterList", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var entity = new DescriptionMaster
                {
                    DescriptionId = Convert.ToInt32(reader["DescriptionId"]),
                    DescriptionCode = reader.GetString("DescriptionCode"), 
                    DescriptionName = reader.GetString("DescriptionName"),
                    Group = reader.GetString("Group"),
                    Section = reader.GetString("Section"),
                    Applicable_All = Convert.ToBoolean(reader["Applicable_All"]),
                    Applicable_LAB = Convert.ToBoolean(reader["Applicable_LAB"]),
                    Applicable_OP = Convert.ToBoolean(reader["Applicable_OP"]),
                    Consultation = Convert.ToBoolean(reader["Consultation"]),
                    NormalCharges = Convert.ToDecimal(reader["NormalCharges"]),
                    EmergencyCharges = Convert.ToDecimal(reader["EmergencyCharges"]),
                    DrCharges = Convert.ToDecimal(reader["DrCharges"]),
                    IsActive = Convert.ToBoolean(reader["IsActive"])
                };
                list.Add(entity);
            }

            // 🔁 AutoMapper: Convert Model → ViewModel
            var vmList = _mapper.Map<List<DescriptionMasterViewModel>>(list);
            return View(vmList);
        }

        // ✅ DETAILS
        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();

            DescriptionMaster description = null;
            using var conn = _db.GetConnection();
            conn.Open();

            using var cmd = new MySqlCommand("GetDescriptionMasterById", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p_DescriptionId", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                description = new DescriptionMaster
                {
                    DescriptionId = Convert.ToInt32(reader["DescriptionId"]),
                    DescriptionCode = reader["DescriptionCode"].ToString(),
                    DescriptionName = reader["DescriptionName"].ToString(),
                    Group = reader["Group"].ToString(),
                    Section = reader["Section"].ToString(),
                    Applicable_All = Convert.ToBoolean(reader["Applicable_All"]),
                    Applicable_LAB = Convert.ToBoolean(reader["Applicable_LAB"]),
                    Applicable_OP = Convert.ToBoolean(reader["Applicable_OP"]),
                    Consultation = Convert.ToBoolean(reader["Consultation"]),
                    NormalCharges = Convert.ToDecimal(reader["NormalCharges"]),
                    EmergencyCharges = Convert.ToDecimal(reader["EmergencyCharges"]),
                    DrCharges = Convert.ToDecimal(reader["DrCharges"]),
                    IsActive = Convert.ToBoolean(reader["IsActive"])
                };
            }

            if (description == null) return NotFound();

            // 🔁 AutoMapper: Model → ViewModel
            var vm = _mapper.Map<DescriptionMasterViewModel>(description);

            // Load test values (2nd result set)
            if (reader.NextResult() && reader.HasRows)
            {
                vm.TestValues = new List<DescriptionMasterTestValueViewModel>();
                while (reader.Read())
                {
                    vm.TestValues.Add(new DescriptionMasterTestValueViewModel
                    {
                        TestValueID = Convert.ToInt32(reader["TestValueID"]),
                        DescriptionId = Convert.ToInt32(reader["DescriptionId"]),
                        Specimen = reader["Specimen"].ToString(),
                        TestName = reader["TestName"].ToString(),
                        Male = reader["Male"].ToString(),
                        Female = reader["Female"].ToString(),
                        Children = reader["Children"].ToString(),
                        General = reader["General"].ToString(),
                        Unit = reader["Unit"].ToString(),
                        Method = reader["Method"].ToString(),
                        HtmlStyle = reader["HtmlStyle"].ToString(),
                        Instrument = reader["Instrument"].ToString(),
                        SortOrder = Convert.ToInt32(reader["SortOrder"])
                    });
                }
            }

            return View(vm);
        }

        // ✅ CREATE (GET)
        public IActionResult Create()
        {
            return View(new DescriptionMasterViewModel());
        }

        // ✅ CREATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DescriptionMasterViewModel viewModel)
        {
            // 🔁 Map ViewModel → Model
            var model = _mapper.Map<DescriptionMaster>(viewModel);

            using var conn = _db.GetConnection();
            conn.Open();
            using var transaction = conn.BeginTransaction();

            try
            {
                long newId;
                using (var cmd = new MySqlCommand("CreateDescriptionMaster", conn, transaction))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    AddMySqlParametersForCreate(cmd, model);
                    using var reader = cmd.ExecuteReader();
                    reader.Read();
                    newId = reader.GetInt64("NewDescriptionId");
                }

                if (viewModel.TestValues != null)
                {
                    foreach (var test in viewModel.TestValues)
                    {
                        using var testCmd = new MySqlCommand("AddDescriptionMasterTestValue", conn, transaction);
                        testCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        AddTestValueParameters(testCmd, newId, test);
                        testCmd.ExecuteNonQuery();
                    }
                }

                transaction.Commit();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error creating DescriptionMaster");
                ModelState.AddModelError("", "Error occurred while saving: " + ex.Message);
                return View(viewModel);
            }
        }

        // ✅ EDIT (GET)
        public IActionResult Edit(int? id) => id == null ? NotFound() : Details(id);

        // ✅ EDIT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, DescriptionMasterViewModel viewModel)
        {
            if (id != viewModel.DescriptionId) return NotFound();

            var model = _mapper.Map<DescriptionMaster>(viewModel);

            using var conn = _db.GetConnection();
            conn.Open();
            using var transaction = conn.BeginTransaction();

            try
            {
                using (var cmd = new MySqlCommand("UpdateDescriptionMaster", conn, transaction))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    AddMySqlParametersForUpdate(cmd, model);
                    cmd.ExecuteNonQuery();
                }

                using (var delCmd = new MySqlCommand("DeleteDescriptionTestValues", conn, transaction))
                {
                    delCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    delCmd.Parameters.AddWithValue("@p_DescriptionId", id);
                    delCmd.ExecuteNonQuery();
                }

                if (viewModel.TestValues != null)
                {
                    foreach (var test in viewModel.TestValues)
                    {
                        using var testCmd = new MySqlCommand("AddDescriptionMasterTestValue", conn, transaction);
                        testCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        AddTestValueParameters(testCmd, id, test);
                        testCmd.ExecuteNonQuery();
                    }
                }

                transaction.Commit();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error editing DescriptionMaster");
                ModelState.AddModelError("", "Error while saving: " + ex.Message);
                return View(viewModel);
            }
        }

        // ✅ DELETE (GET)
       public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            var description = Details(id) as ViewResult;
            return description != null ? description : NotFound();
        }


        // ✅ DELETE (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            try
            {
                using var cmd = new MySqlCommand("DeleteDescriptionMaster", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@p_DescriptionId", id);
                cmd.ExecuteNonQuery();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting DescriptionMaster");
                return RedirectToAction(nameof(Index));
            }
        }

        // ✅ Helper Methods
        private static void AddMySqlParametersForCreate(MySqlCommand cmd, DescriptionMaster d)
        {
            cmd.Parameters.AddWithValue("@p_DescriptionCode", d.DescriptionCode ?? "");
            cmd.Parameters.AddWithValue("@p_DescriptionName", d.DescriptionName ?? "");
            cmd.Parameters.AddWithValue("@p_Group", d.Group ?? "");
            cmd.Parameters.AddWithValue("@p_Section", d.Section ?? "");
            cmd.Parameters.AddWithValue("@p_Applicable_All", d.Applicable_All);
            cmd.Parameters.AddWithValue("@p_Applicable_LAB", d.Applicable_LAB);
            cmd.Parameters.AddWithValue("@p_Applicable_OP", d.Applicable_OP);
            cmd.Parameters.AddWithValue("@p_Consultation", d.Consultation);
            cmd.Parameters.AddWithValue("@p_NormalCharges", d.NormalCharges);
            cmd.Parameters.AddWithValue("@p_EmergencyCharges", d.EmergencyCharges);
            cmd.Parameters.AddWithValue("@p_DrCharges", d.DrCharges);
            cmd.Parameters.AddWithValue("@p_IsActive", d.IsActive);
        }

        private static void AddMySqlParametersForUpdate(MySqlCommand cmd, DescriptionMaster d)
        {
            cmd.Parameters.AddWithValue("@p_DescriptionId", d.DescriptionId);
            AddMySqlParametersForCreate(cmd, d);
        }

        private static void AddTestValueParameters(MySqlCommand cmd, long descriptionId, DescriptionMasterTestValueViewModel t)
        {
            cmd.Parameters.AddWithValue("@p_DescriptionId", descriptionId);
            cmd.Parameters.AddWithValue("@p_Specimen", t.Specimen ?? "");
            cmd.Parameters.AddWithValue("@p_TestName", t.TestName ?? "");
            cmd.Parameters.AddWithValue("@p_Male", t.Male ?? "");
            cmd.Parameters.AddWithValue("@p_Female", t.Female ?? "");
            cmd.Parameters.AddWithValue("@p_Children", t.Children ?? "");
            cmd.Parameters.AddWithValue("@p_General", t.General ?? "");
            cmd.Parameters.AddWithValue("@p_Unit", t.Unit ?? "");
            cmd.Parameters.AddWithValue("@p_Method", t.Method ?? "");
            cmd.Parameters.AddWithValue("@p_HtmlStyle", t.HtmlStyle ?? "");
            cmd.Parameters.AddWithValue("@p_Instrument", t.Instrument ?? "");
            cmd.Parameters.AddWithValue("@p_SortOrder", t.SortOrder);
        }
    }
}
