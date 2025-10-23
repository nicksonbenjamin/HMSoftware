using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClinicApp.ViewModels
{
    public class ApplicationUserViewModel
    {
        public Guid UserId { get; set; }
        public string LoginNamee { get; set; }
        public Guid RoleID { get; set; }
        public string LoginPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string RoleName { get; set; }
        public List<SelectListItem> Roles { get; set; }
        public string ErrorMessage { get; set; }
    }
}