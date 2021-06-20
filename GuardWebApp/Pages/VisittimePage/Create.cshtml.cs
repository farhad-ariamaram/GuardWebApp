using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GuardWebApp.Models;

namespace GuardWebApp.Pages.VisittimePage
{
    public class CreateModel : PageModel
    {
        private readonly GuardWebApp.Models.GuardianDBContext _context;

        public CreateModel(GuardWebApp.Models.GuardianDBContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Visittime Visittime { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Visittime.StartDate = new DateTime(2000, int.Parse(Request.Form["startdatemonth"].ToString()), int.Parse(Request.Form["startdateday"].ToString()));
            Visittime.EndDate = new DateTime(2000, int.Parse(Request.Form["enddatemonth"].ToString()), int.Parse(Request.Form["enddateday"].ToString()));

            _context.Visittimes.Add(Visittime);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
