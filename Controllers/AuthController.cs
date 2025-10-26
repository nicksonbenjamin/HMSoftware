using Microsoft.AspNetCore.Mvc;
using ClinicApp.ViewModels;
using System.Linq;
using Microsoft.AspNetCore.Http;
using ClinicApp.Data; // Add this line if ClinicAppMySqlDbContext is in ClinicApp.Data namespace
using MySql.Data.MySqlClient;

namespace ClinicApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        public ApplicationUserViewModel _appUserVM;
        private readonly ClinicAppMySqlDbContext _db;
        public AuthController(ILogger<AuthController> logger, ClinicAppMySqlDbContext db, ApplicationUserViewModel appUserVM)
        {
            _logger = logger;
            _appUserVM = appUserVM;
            _db = db;
            _appUserVM.ErrorMessage = string.Empty;
        }

        // GET: /Auth/Login
        [HttpGet]
        public IActionResult Login()
        {
            _appUserVM.ErrorMessage = string.Empty;
            return View(_appUserVM);
        }

        // POST: /Auth/Login
        [HttpPost]
        public IActionResult Login(ApplicationUserViewModel model)
        {
            // if (!ModelState.IsValid)
            //   return View(model);
            if (model != null)
            {
                // Login success - save info in session
                if (string.IsNullOrEmpty(model.LoginNamee) || string.IsNullOrEmpty(model.LoginPassword))
                {
                    model.ErrorMessage = "Username and Password are required.";
                    return View(model);
                }

                using var connection = _db.GetConnection();
                connection.Open();
                var cmd = new MySqlCommand("SELECT * FROM ApplicationUser WHERE LoginNamee = @username AND LoginPassword = @password", connection);
                cmd.Parameters.AddWithValue("@username", model.LoginNamee);
                cmd.Parameters.AddWithValue("@password", model.LoginPassword);
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {

                    HttpContext.Session.SetString("UserId", Convert.ToString(reader.GetGuid("UserId")));
                    HttpContext.Session.SetString("UserName", reader.GetString("FirstName") + " " + reader.GetString("LastName"));
                    HttpContext.Session.SetString("RoleId", reader.GetGuid("RoleID").ToString() ?? "Unknown");
                    // Login successful, redirect to dashboard or home
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    model.ErrorMessage = "Invalid username or password.";
                    return View(model);
                }
            }
            else
            {
                _appUserVM.ErrorMessage = "Invalid login attempt.";
                return View(_appUserVM);
            }
        }

        // GET: /Auth/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
