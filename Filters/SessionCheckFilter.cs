using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class SessionCheckFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var session = context.HttpContext.Session.GetString("UserName");

        // Get current controller and action names
        var controller = context.RouteData.Values["controller"]?.ToString();
        var action = context.RouteData.Values["action"]?.ToString();

        // Skip the session check for Login controller/action or any other you want to allow without login
        if (controller == "Auth" && action == "Login")
        {
            return; // allow Login action without redirect
        }

        if (string.IsNullOrEmpty(session))
        {
            context.Result = new RedirectToActionResult("Login", "Auth", null);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}
