using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySql.Data.MySqlClient;
using ClinicApp.Data;

namespace ClinicApp.ApplicationUsers
{
    public class EditModel : PageModel
    {
        private readonly ClinicAppMySqlDbContext _db;
        [BindProperty] public ApplicationUser User { get; set; }
        public List<SelectListItem> Roles { get; set; }

        public EditModel(ClinicAppMySqlDbContext db)
        {
            _db = db;
        }

        public void OnGet(string id)
        {
            Roles = new List<SelectListItem>();
            using var conn = _db.GetConnection();
            conn.Open();

            var roleCmd = new MySqlCommand("SELECT RoleId, Name FROM Roles", conn);
            using var roleReader = roleCmd.ExecuteReader();
            while (roleReader.Read())
            {
                Roles.Add(new SelectListItem
                {
                    Value = roleReader.GetString("RoleId"),
                    Text = roleReader.GetString("Name")
                });
            }
            roleReader.Close();

            var cmd = new MySqlCommand("SELECT * FROM ApplicationUser WHERE UserId=@Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                User = new ApplicationUser
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
        }

        public IActionResult OnPost()
        {
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand(@"UPDATE ApplicationUser SET 
            LoginNamee=@LoginNamee, RoleID=@RoleID, LoginPassword=@LoginPassword, 
            FirstName=@FirstName, LastName=@LastName, MiddleName=@MiddleName 
            WHERE UserId=@UserId", conn);
            cmd.Parameters.AddWithValue("@UserId", User.UserId);
            cmd.Parameters.AddWithValue("@LoginNamee", User.LoginNamee);
            cmd.Parameters.AddWithValue("@RoleID", User.RoleID);
            cmd.Parameters.AddWithValue("@LoginPassword", User.LoginPassword);
            cmd.Parameters.AddWithValue("@FirstName", User.FirstName);
            cmd.Parameters.AddWithValue("@LastName", User.LastName);
            cmd.Parameters.AddWithValue("@MiddleName", User.MiddleName);
            cmd.ExecuteNonQuery();
            return RedirectToPage("Index");
        }
    }
}