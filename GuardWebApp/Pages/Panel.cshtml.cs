using GuardWebApp.Controllers;
using GuardWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static GuardWebApp.Controllers.UsersController;

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
            var a =  await api.Attend("199","199", new DateTime(2021,05,23).ToShortDateString());

            return Page();
        }
    }
}
