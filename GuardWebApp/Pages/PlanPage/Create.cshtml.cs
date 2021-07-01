using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GuardWebApp.Models;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System;

namespace GuardWebApp.Pages.PlanPage
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
            var uid = HttpContext.Session.GetString("uid");
            if (uid == null)
            {
                return RedirectToPage("../Index");
            }

            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Name");
            ViewData["ShiftId"] = new SelectList(_context.Shifts, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Plan Plan { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            PersianCalendar pc = new PersianCalendar();
            var time = TimeSpan.Parse(Request.Form["timeField"].ToString());
            Plan.DateTime = new System.DateTime(1400, int.Parse(Request.Form["monthField"].ToString()), int.Parse(Request.Form["dayField"].ToString()), time.Hours, time.Minutes, time.Seconds, pc);

            _context.Plans.Add(Plan);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
