using GuardWebApp.Controllers;
using GuardWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace GuardWebApp.Pages
{
    public class PanelModel : PageModel
    {
        public async Task<IActionResult> OnGet()
        {
            GuardianDBContext _db = new GuardianDBContext();

            var uid = HttpContext.Session.GetString("uid");
            if (uid == null)
            {
                return RedirectToPage("./Index");
            }

            UsersController api = new UsersController(_db);
            await api.Attend(uid, DateTime.Now.ToString(), "199");

            return Page();
        }
    }
}
