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
    public class CommonMasterController : Controller
    {
        private readonly ILogger<CommonMasterController> _logger;
        private readonly ClinicAppMySqlDbContext _db;
        private readonly IMapper _mapper;

        public CommonMasterController(
            ILogger<CommonMasterController> logger,
            ClinicAppMySqlDbContext db,
            IMapper mapper)
        {
            _logger = logger;
            _db = db;
            _mapper = mapper;
        }

        // INDEX - Get all CommonMaster records
        public IActionResult Index()
        {
            var list = new List<CommonMaster>();
            using var conn = _db.GetConnection();
            conn.Open();

            using var cmd = new MySqlCommand("GetCommonMasterList", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var entity = new CommonMaster
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    MasterType = reader.GetString("MasterType"),
                    MasterName = reader.GetString("MasterName")
                };
                list.Add(entity);
            }

            var vmList = _mapper.Map<List<CommonMasterViewModel>>(list);
            return View(vmList);
        }

        // DETAILS
        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();

            CommonMaster entity = null;
            using var conn = _db.GetConnection();
            conn.Open();

            using var cmd = new MySqlCommand("GetCommonMasterById", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p_Id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                entity = new CommonMaster
                {
                    Id = Convert.ToInt32(reader["Id"]),
                     MasterType = reader.GetString("MasterType"),
                    MasterName = reader.GetString("MasterName")
                };
            }

            if (entity == null) return NotFound();

            var vm = _mapper.Map<CommonMasterViewModel>(entity);
            return View(vm);
        }

        // CREATE (GET)
        public IActionResult Create()
        {
            return View(new CommonMasterViewModel());
        }

        // CREATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CommonMasterViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            var model = _mapper.Map<CommonMaster>(viewModel);

            using var conn = _db.GetConnection();
            conn.Open();
            using var transaction = conn.BeginTransaction();

            try
            {
                using var cmd = new MySqlCommand("CreateCommonMaster", conn, transaction);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@p_MasterType", model.MasterType ?? "");
                cmd.Parameters.AddWithValue("@p_MasterName", model.MasterName ?? "");
                cmd.ExecuteNonQuery();

                transaction.Commit();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error creating CommonMaster");
                ModelState.AddModelError("", "Error occurred while saving: " + ex.Message);
                return View(viewModel);
            }
        }

        // EDIT (GET)
        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();

            using var conn = _db.GetConnection();
            conn.Open();
            CommonMaster entity = null;

            using var cmd = new MySqlCommand("GetCommonMasterById", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p_Id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                entity = new CommonMaster
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    MasterType = reader.GetString("MasterType"),
                    MasterName = reader.GetString("MasterName")
                };
            }

            if (entity == null) return NotFound();

            var vm = _mapper.Map<CommonMasterViewModel>(entity);
            return View(vm);
        }

        // EDIT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, CommonMasterViewModel viewModel)
        {
            if (id != viewModel.Id) return NotFound();
            if (!ModelState.IsValid) return View(viewModel);

            var model = _mapper.Map<CommonMaster>(viewModel);

            using var conn = _db.GetConnection();
            conn.Open();
            using var transaction = conn.BeginTransaction();

            try
            {
                using var cmd = new MySqlCommand("UpdateCommonMaster", conn, transaction);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@p_Id", model.Id);
                cmd.Parameters.AddWithValue("@p_MasterType", model.MasterType ?? "");
                cmd.Parameters.AddWithValue("@p_MasterName", model.MasterName ?? "");
                cmd.ExecuteNonQuery();

                transaction.Commit();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error editing CommonMaster");
                ModelState.AddModelError("", "Error while saving: " + ex.Message);
                return View(viewModel);
            }
        }

        // DELETE (GET)
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            using var conn = _db.GetConnection();
            conn.Open();
            CommonMaster entity = null;

            using var cmd = new MySqlCommand("GetCommonMasterById", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p_Id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                entity = new CommonMaster
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    MasterType = reader.GetString("MasterType"),
                    MasterName = reader.GetString("MasterName")
                };
            }

            if (entity == null) return NotFound();

            var vm = _mapper.Map<CommonMasterViewModel>(entity);
            return View(vm);
        }

        // DELETE (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            try
            {
                using var cmd = new MySqlCommand("DeleteCommonMaster", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@p_Id", id);
                cmd.ExecuteNonQuery();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting CommonMaster");
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
