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
    public class ICDCodeMasterController : Controller
    {
        private readonly ILogger<ICDCodeMasterController> _logger;
        private readonly ClinicAppMySqlDbContext _db;
        private readonly IMapper _mapper;

        public ICDCodeMasterController(
            ILogger<ICDCodeMasterController> logger,
            ClinicAppMySqlDbContext db,
            IMapper mapper)
        {
            _logger = logger;
            _db = db;
            _mapper = mapper;
        }

        // ✅ INDEX - Get all ICD codes
        public IActionResult Index()
        {
            var list = new List<ICDCodeMaster>();
            using var conn = _db.GetConnection();
            conn.Open();

            using var cmd = new MySqlCommand("GetICDCodeMasterList", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var entity = new ICDCodeMaster
                {
                    ICDCodeMasterId = Convert.ToInt32(reader["ICDCodeMasterId"]),
                    ICDCode = reader["ICDCode"].ToString(),
                    DiagnosisCondition = reader["DiagnosisCondition"].ToString(),
                    DescriptionUsage = reader["DescriptionUsage"].ToString(),
                    IsActive = Convert.ToBoolean(reader["IsActive"]),
                    CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                    UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
                };
                list.Add(entity);
            }

            var vmList = _mapper.Map<List<Icd10CodeViewModel>>(list);
            return View(vmList);
        }

        // ✅ DETAILS
        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();

            ICDCodeMaster entity = null;
            using var conn = _db.GetConnection();
            conn.Open();

            using var cmd = new MySqlCommand("GetICDCodeMasterById", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p_ICDCodeMasterId", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                entity = new ICDCodeMaster
                {
                    ICDCodeMasterId = Convert.ToInt32(reader["ICDCodeMasterId"]),
                    ICDCode = reader["ICDCode"].ToString(),
                    DiagnosisCondition = reader["DiagnosisCondition"].ToString(),
                    DescriptionUsage = reader["DescriptionUsage"].ToString(),
                    IsActive = Convert.ToBoolean(reader["IsActive"]),
                    CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                    UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
                };
            }

            if (entity == null) return NotFound();

            var vm = _mapper.Map<Icd10CodeViewModel>(entity);
            return View(vm);
        }

        // ✅ CREATE (GET)
        public IActionResult Create()
        {
            return View(new Icd10CodeViewModel());
        }

        // ✅ CREATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Icd10CodeViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            var model = _mapper.Map<ICDCodeMaster>(viewModel);

            using var conn = _db.GetConnection();
            conn.Open();
            using var transaction = conn.BeginTransaction();

            try
            {
                using var cmd = new MySqlCommand("CreateICDCodeMaster", conn, transaction);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@p_ICDCode", model.ICDCode ?? "");
                cmd.Parameters.AddWithValue("@p_DiagnosisCondition", model.DiagnosisCondition ?? "");
                cmd.Parameters.AddWithValue("@p_DescriptionUsage", model.DescriptionUsage ?? "");
                cmd.Parameters.AddWithValue("@p_IsActive", model.IsActive);
                cmd.ExecuteNonQuery();

                transaction.Commit();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error creating ICDCodeMaster");
                ModelState.AddModelError("", "Error occurred while saving: " + ex.Message);
                return View(viewModel);
            }
        }

        // ✅ EDIT (GET)
        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();

            using var conn = _db.GetConnection();
            conn.Open();
            ICDCodeMaster entity = null;

            using var cmd = new MySqlCommand("GetICDCodeMasterById", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p_ICDCodeMasterId", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                entity = new ICDCodeMaster
                {
                    ICDCodeMasterId = Convert.ToInt32(reader["ICDCodeMasterId"]),
                    ICDCode = reader["ICDCode"].ToString(),
                    DiagnosisCondition = reader["DiagnosisCondition"].ToString(),
                    DescriptionUsage = reader["DescriptionUsage"].ToString(),
                    IsActive = Convert.ToBoolean(reader["IsActive"]),
                    CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                    UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
                };
            }

            if (entity == null) return NotFound();

            var vm = _mapper.Map<Icd10CodeViewModel>(entity);
            return View(vm);
        }

        // ✅ EDIT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Icd10CodeViewModel viewModel)
        {
            if (id != viewModel.ICDCodeMasterId) return NotFound();
            if (!ModelState.IsValid) return View(viewModel);

            var model = _mapper.Map<ICDCodeMaster>(viewModel);

            using var conn = _db.GetConnection();
            conn.Open();
            using var transaction = conn.BeginTransaction();

            try
            {
                using var cmd = new MySqlCommand("UpdateICDCodeMaster", conn, transaction);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@p_ICDCodeMasterId", model.ICDCodeMasterId);
                cmd.Parameters.AddWithValue("@p_ICDCode", model.ICDCode ?? "");
                cmd.Parameters.AddWithValue("@p_DiagnosisCondition", model.DiagnosisCondition ?? "");
                cmd.Parameters.AddWithValue("@p_DescriptionUsage", model.DescriptionUsage ?? "");
                cmd.Parameters.AddWithValue("@p_IsActive", model.IsActive);
                cmd.ExecuteNonQuery();

                transaction.Commit();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error editing ICDCodeMaster");
                ModelState.AddModelError("", "Error while saving: " + ex.Message);
                return View(viewModel);
            }
        }

        // ✅ DELETE (GET)
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            using var conn = _db.GetConnection();
            conn.Open();
            ICDCodeMaster entity = null;

            using var cmd = new MySqlCommand("GetICDCodeMasterById", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p_ICDCodeMasterId", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                entity = new ICDCodeMaster
                {
                    ICDCodeMasterId = Convert.ToInt32(reader["ICDCodeMasterId"]),
                    ICDCode = reader["ICDCode"].ToString(),
                    DiagnosisCondition = reader["DiagnosisCondition"].ToString(),
                    DescriptionUsage = reader["DescriptionUsage"].ToString(),
                    IsActive = Convert.ToBoolean(reader["IsActive"]),
                    CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                    UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
                };
            }

            if (entity == null) return NotFound();

            var vm = _mapper.Map<Icd10CodeViewModel>(entity);
            return View(vm);
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
                using var cmd = new MySqlCommand("DeleteICDCodeMaster", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@p_ICDCodeMasterId", id);
                cmd.ExecuteNonQuery();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting ICDCodeMaster");
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
