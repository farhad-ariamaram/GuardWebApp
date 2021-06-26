using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GuardWebApp.Pages
{
    public class LogoutModel : PageModel
    {
        public void OnGet()
        {
            HttpContext.Session.Clear();
        }
    }
}
