using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ClinicApp.Models;
using ClinicApp.ViewModels;
using ClinicApp.Data;
using MySql.Data.MySqlClient;

namespace ClinicApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    public ApplicationUserViewModel _appUserVM;
    private readonly ClinicAppMySqlDbContext _db;
    private readonly AutoMapper.IMapper _mapper;

    public HomeController(ILogger<HomeController> logger, ClinicAppMySqlDbContext db, ApplicationUserViewModel appUserVM, AutoMapper.IMapper mapper)
        {
            _logger = logger;
            _appUserVM = appUserVM;
            _db = db;
            _mapper = mapper;
            _appUserVM.ErrorMessage = string.Empty;
        }

    public IActionResult Index()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
        {
            return RedirectToAction("Login", "Auth");
        }

        ViewBag.UserName = HttpContext.Session.GetString("UserName");
        /*
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
        */
        return RedirectToAction("Index", "ApplicationUsers");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
