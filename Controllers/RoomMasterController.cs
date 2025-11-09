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
    public class RoomMasterController : Controller
    {
        private readonly ILogger<RoomMasterController> _logger;
        private readonly ClinicAppMySqlDbContext _db;
        private readonly IMapper _mapper;

        public RoomMasterController(
            ILogger<RoomMasterController> logger,
            ClinicAppMySqlDbContext db,
            IMapper mapper)
        {
            _logger = logger;
            _db = db;
            _mapper = mapper;
        }

        // ===================================
        // ✅ INDEX (List all rooms)
        // ===================================
        public IActionResult Index()
        {
            var list = new List<RoomMaster>();

            using var conn = _db.GetConnection();
            conn.Open();

            using var cmd = new MySqlCommand("GetRoomMasterList", conn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(ReadRoomMaster(reader));
            }

            var vmList = _mapper.Map<List<RoomMasterViewModel>>(list);
            return View(vmList);
        }

        // ===================================
        // ✅ DETAILS
        // ===================================
        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();

            RoomMaster entity = null;
            using var conn = _db.GetConnection();
            conn.Open();

            using var cmd = new MySqlCommand("GetRoomMasterById", conn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@p_RoomId", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                entity = ReadRoomMaster(reader);
            }

            if (entity == null) return NotFound();

            var vm = _mapper.Map<RoomMasterViewModel>(entity);
            vm.RoomTypeList = LoadRoomTypeList();
            return View(vm);
        }

        // ===================================
        // ✅ CREATE (GET)
        // ===================================
        public IActionResult Create()
        {
            var vm = new RoomMasterViewModel
            {
                RoomTypeList = LoadRoomTypeList()
            };
            return View(vm);
        }

        // ===================================
        // ✅ CREATE (POST)
        // ===================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RoomMasterViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.RoomTypeList = LoadRoomTypeList();
                return View(viewModel);
            }

            var model = _mapper.Map<RoomMaster>(viewModel);

            try
            {
                using var conn = _db.GetConnection();
                conn.Open();

                using var cmd = new MySqlCommand("CreateRoomMaster", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                AddMySqlParametersForCreate(cmd, model);
                cmd.ExecuteNonQuery();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating RoomMaster");
                ModelState.AddModelError("", "Error while saving: " + ex.Message);
                viewModel.RoomTypeList = LoadRoomTypeList();
                return View(viewModel);
            }
        }

        // ===================================
        // ✅ EDIT (GET)
        // ===================================
        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();
            return Details(id);
        }

        // ===================================
        // ✅ EDIT (POST)
        // ===================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, RoomMasterViewModel viewModel)
        {
            if (id != viewModel.RoomId) return NotFound();

            var model = _mapper.Map<RoomMaster>(viewModel);

            try
            {
                using var conn = _db.GetConnection();
                conn.Open();

                using var cmd = new MySqlCommand("UpdateRoomMaster", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                AddMySqlParametersForUpdate(cmd, model);
                cmd.ExecuteNonQuery();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating RoomMaster");
                ModelState.AddModelError("", "Error while updating: " + ex.Message);
                viewModel.RoomTypeList = LoadRoomTypeList();
                return View(viewModel);
            }
        }

        // ===================================
        // ✅ DELETE (GET)
        // ===================================
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            var roomView = Details(id) as ViewResult;
            if (roomView == null) return NotFound();

            return roomView;
        }

        // ===================================
        // ✅ DELETE (POST)
        // ===================================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                using var conn = _db.GetConnection();
                conn.Open();

                using var cmd = new MySqlCommand("DeleteRoomMaster", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@p_RoomId", id);
                cmd.ExecuteNonQuery();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting RoomMaster");
                return RedirectToAction(nameof(Index));
            }
        }

        // ===================================
        // ✅ Helper Methods
        // ===================================

        private static RoomMaster ReadRoomMaster(MySqlDataReader reader)
        {
            return new RoomMaster
            {
                RoomId = Convert.ToInt32(reader["RoomId"]),
                RoomNo = reader["RoomNo"].ToString(),
                FloorNo = reader["FloorNo"].ToString(),
                TypeId = Convert.ToInt32(reader["TypeId"]),
                NoOfBeds = Convert.ToInt32(reader["NoOfBeds"]),
                RentPerDay = Convert.ToDecimal(reader["RentPerDay"]),
                RentPerHour = Convert.ToDecimal(reader["RentPerHour"]),
                NursingChargePerDay = Convert.ToDecimal(reader["NursingChargePerDay"]),
                Remarks = reader["Remarks"].ToString(),
                ChargeDescription = reader["ChargeDescription"].ToString(),
                AmountPerDay = Convert.ToDecimal(reader["AmountPerDay"]),
                IsActive = Convert.ToBoolean(reader["IsActive"])
            };
        }

        private static void AddMySqlParametersForCreate(MySqlCommand cmd, RoomMaster r)
        {
            cmd.Parameters.AddWithValue("@p_RoomNo", r.RoomNo ?? "");
            cmd.Parameters.AddWithValue("@p_FloorNo", r.FloorNo ?? "");
            cmd.Parameters.AddWithValue("@p_TypeId", r.TypeId);
            cmd.Parameters.AddWithValue("@p_NoOfBeds", r.NoOfBeds);
            cmd.Parameters.AddWithValue("@p_RentPerDay", r.RentPerDay);
            cmd.Parameters.AddWithValue("@p_RentPerHour", r.RentPerHour);
            cmd.Parameters.AddWithValue("@p_NursingChargePerDay", r.NursingChargePerDay);
            cmd.Parameters.AddWithValue("@p_Remarks", r.Remarks ?? "");
            cmd.Parameters.AddWithValue("@p_ChargeDescription", r.ChargeDescription ?? "");
            cmd.Parameters.AddWithValue("@p_AmountPerDay", r.AmountPerDay);
            cmd.Parameters.AddWithValue("@p_IsActive", r.IsActive);
        }

        private static void AddMySqlParametersForUpdate(MySqlCommand cmd, RoomMaster r)
        {
            cmd.Parameters.AddWithValue("@p_RoomId", r.RoomId);
            AddMySqlParametersForCreate(cmd, r);
        }

        private List<RoomTypeMaster> LoadRoomTypeList()
        {
            var list = new List<RoomTypeMaster>();

            using var conn = _db.GetConnection();
            conn.Open();

            using var cmd = new MySqlCommand("GetRoomTypeMasterList", conn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new RoomTypeMaster
                {
                    TypeId = Convert.ToInt32(reader["TypeId"]),
                    TypeName = reader["TypeName"].ToString()
                });
            }

            return list;
        }
    }
}
