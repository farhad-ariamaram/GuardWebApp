using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GuardWebApp.Models;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System;

namespace GuardWebApp.Pages.PlanPage
{
    public class EditModel : PageModel
    {
        private readonly GuardWebApp.Models.GuardianDBContext _context;

        public EditModel(GuardWebApp.Models.GuardianDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Plan Plan { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            var uid = HttpContext.Session.GetString("uid");
            if (uid == null)
            {
                return RedirectToPage("../Index");
            }

            if (id == null)
            {
                return NotFound();
            }

            Plan = await _context.Plans
                .Include(p => p.Location)
                .Include(p => p.Shift)
                .Include(p => p.User).FirstOrDefaultAsync(m => m.Id == id);

            if (Plan == null)
            {
                return NotFound();
            }
           ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Name");
           ViewData["ShiftId"] = new SelectList(_context.Shifts, "Id", "Id");
           ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            PersianCalendar pc = new PersianCalendar();
            var time = TimeSpan.Parse(Request.Form["timeField"].ToString());
            Plan.DateTime = new System.DateTime(1400, int.Parse(Request.Form["monthField"].ToString()), int.Parse(Request.Form["dayField"].ToString()), time.Hours, time.Minutes, time.Seconds, pc);

            _context.Attach(Plan).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlanExists(Plan.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool PlanExists(long id)
        {
            return _context.Plans.Any(e => e.Id == id);
        }
    }
}
