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
    public class ApplicationUsersController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        public ApplicationUserViewModel _appUserVM;
        private readonly ClinicAppMySqlDbContext _db;
        private readonly AutoMapper.IMapper _mapper;
        public ApplicationUsersController(ILogger<AuthController> logger, ClinicAppMySqlDbContext db, ApplicationUserViewModel appUserVM, AutoMapper.IMapper mapper)
        {
            _logger = logger;
            _appUserVM = appUserVM;
            _db = db;
            _appUserVM.ErrorMessage = string.Empty;
            _mapper = mapper;
        }

        // GET: ApplicationUsers
        public IActionResult Index()
        {
            List<ApplicationUser> Users = new List<ApplicationUser>();
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand(@"SELECT u.*, r.RoleName
            FROM ApplicationUser u
            LEFT JOIN Roles r ON u.RoleID = r.RoleId", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Users.Add(new ApplicationUser
                {
                    UserId = reader.GetGuid("UserId"),
                    LoginNamee = reader.GetString("LoginNamee"),
                    RoleID = reader.GetGuid("RoleID"),
                    LoginPassword = reader.GetString("LoginPassword"),
                    FirstName = reader.GetString("FirstName"),
                    LastName = reader.GetString("LastName"),
                    MiddleName = reader.GetString("MiddleName"),
                    RoleName = reader.IsDBNull(reader.GetOrdinal("RoleName")) ? "" : reader.GetString("RoleName")
                });
            }
            List<ApplicationUserViewModel> appUserVMList = new List<ApplicationUserViewModel>();
            foreach (var user in Users)
            {
                ApplicationUserViewModel applicationUserViewModel = _mapper.Map<ApplicationUserViewModel>(user);
                appUserVMList.Add(applicationUserViewModel);
            }

            return View(appUserVMList);
        }

        // GET: ApplicationUsers/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ApplicationUser user = null;
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand(@"
            SELECT u.*, r.RoleName
            FROM ApplicationUser u
            LEFT JOIN Roles r ON u.RoleID = r.RoleId
            WHERE u.UserId=@Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                user = new ApplicationUser
                {
                    UserId = reader.GetGuid("UserId"),
                    LoginNamee = reader.GetString("LoginNamee"),
                    RoleID = reader.GetGuid("RoleID"),
                    LoginPassword = reader.GetString("LoginPassword"),
                    FirstName = reader.GetString("FirstName"),
                    LastName = reader.GetString("LastName"),
                    MiddleName = reader.GetString("MiddleName"),
                    RoleName = reader.IsDBNull(reader.GetOrdinal("RoleName")) ? "" : reader.GetString("RoleName")
                };
            }
            ApplicationUserViewModel appUserVM = _mapper.Map<ApplicationUserViewModel>(user);
            return View(appUserVM);
        }

        // GET: ApplicationUsers/Create
        public IActionResult Create()
        {
            List<SelectListItem> roles = new List<SelectListItem>();
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand("SELECT RoleId, RoleName FROM Roles", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                roles.Add(new SelectListItem
                {
                    Value = reader.GetGuid("RoleId").ToString(),
                    Text = reader.GetString("RoleName")
                });
            }
            _appUserVM.Roles = roles;
            //ViewData["RoleID"] = new SelectList(_context.Roles, "RoleId", "RoleId");
            return View(_appUserVM);
        }

        // POST: ApplicationUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("UserId,LoginNamee,RoleID,LoginPassword,FirstName,LastName,MiddleName")] ApplicationUser applicationUser)
        {
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
            applicationUser.UserId = Guid.NewGuid();
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand(@"INSERT INTO ApplicationUser 
                (UserId, LoginNamee, RoleID, LoginPassword, FirstName, LastName, MiddleName) 
                VALUES (@UserId, @LoginNamee, @RoleID, @LoginPassword, @FirstName, @LastName, @MiddleName)", conn);
            cmd.Parameters.AddWithValue("@UserId", applicationUser.UserId);
            cmd.Parameters.AddWithValue("@LoginNamee", applicationUser.LoginNamee);
            cmd.Parameters.AddWithValue("@RoleID", applicationUser.RoleID);
            cmd.Parameters.AddWithValue("@LoginPassword", applicationUser.LoginPassword);
            cmd.Parameters.AddWithValue("@FirstName", applicationUser.FirstName);
            cmd.Parameters.AddWithValue("@LastName", applicationUser.LastName);
            cmd.Parameters.AddWithValue("@MiddleName", applicationUser.MiddleName);
            cmd.ExecuteNonQuery();
            return RedirectToAction("Index");
            //}
            //ViewData["RoleID"] = new SelectList(_context.Roles, "RoleId", "RoleId", applicationUser.RoleID);
        }

        // GET: ApplicationUsers/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            List<SelectListItem> Roles = new List<SelectListItem>();
            using var conn = _db.GetConnection();
            conn.Open();

            var roleCmd = new MySqlCommand("SELECT RoleId, RoleName FROM Roles", conn);
            using var roleReader = roleCmd.ExecuteReader();
            while (roleReader.Read())
            {
                Roles.Add(new SelectListItem
                {
                    Value = Convert.ToString(roleReader.GetGuid("RoleId")),
                    Text = roleReader.GetString("RoleName")
                });
            }
            roleReader.Close();

            ApplicationUser user = null;
            var cmd = new MySqlCommand("SELECT * FROM ApplicationUser WHERE UserId=@Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                user = new ApplicationUser
                {
                    UserId = reader.GetGuid("UserId"),
                    LoginNamee = reader.GetString("LoginNamee"),
                    RoleID = reader.GetGuid("RoleID"),
                    LoginPassword = reader.GetString("LoginPassword"),
                    FirstName = reader.GetString("FirstName"),
                    LastName = reader.GetString("LastName"),
                    MiddleName = reader.GetString("MiddleName")
                };
            }

            ApplicationUserViewModel appUserVM = _mapper.Map<ApplicationUserViewModel>(user);
            appUserVM.Roles = Roles;
            return View(appUserVM);
            //ViewData["RoleID"] = new SelectList(_context.Roles, "RoleId", "RoleId", applicationUser.RoleID);
        }

        // POST: ApplicationUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("UserId,LoginNamee,RoleID,LoginPassword,FirstName,LastName,MiddleName")] ApplicationUser applicationUser)
        {
            if (!Guid.Equals(id, applicationUser.UserId))
            {
                return NotFound();
            }
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand(@"UPDATE ApplicationUser SET 
            LoginNamee=@LoginNamee, RoleID=@RoleID, LoginPassword=@LoginPassword, 
            FirstName=@FirstName, LastName=@LastName, MiddleName=@MiddleName 
            WHERE UserId=@UserId", conn);
            cmd.Parameters.AddWithValue("@UserId", applicationUser.UserId);
            cmd.Parameters.AddWithValue("@LoginNamee", applicationUser.LoginNamee);
            cmd.Parameters.AddWithValue("@RoleID", applicationUser.RoleID);
            cmd.Parameters.AddWithValue("@LoginPassword", applicationUser.LoginPassword);
            cmd.Parameters.AddWithValue("@FirstName", applicationUser.FirstName);
            cmd.Parameters.AddWithValue("@LastName", applicationUser.LastName);
            cmd.Parameters.AddWithValue("@MiddleName", applicationUser.MiddleName);
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

        // GET: ApplicationUsers/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ApplicationUser user = null;
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand("SELECT * FROM ApplicationUser WHERE UserId=@Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                user = new ApplicationUser
                {
                    UserId = reader.GetGuid("UserId"),
                    LoginNamee = reader.GetString("LoginNamee"),
                    FirstName = reader.GetString("FirstName"),
                    MiddleName = reader.GetString("MiddleName"),
                    LastName = reader.GetString("LastName")
                };
            }
            ApplicationUserViewModel appUserVM = _mapper.Map<ApplicationUserViewModel>(user);
            return View(appUserVM);
        }

        // POST: ApplicationUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand("DELETE FROM ApplicationUser WHERE UserId=@UserId", conn);
            cmd.Parameters.AddWithValue("@UserId", id);
            cmd.ExecuteNonQuery();
            return RedirectToAction(nameof(Index));
        }
    }
}
