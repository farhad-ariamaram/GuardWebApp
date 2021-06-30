using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GuardWebApp.Models;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace GuardWebApp.Pages.ShiftPage
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

            ViewData["GuardAreaId"] = new SelectList(_context.GuardAreas, "Id", "Description");
            ViewData["RhythmId"] = new SelectList(_context.Rhythms, "Id", "Title");
            return Page();
        }

        [BindProperty]
        public Shift Shift { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            PersianCalendar pc = new PersianCalendar();
            Shift.DateTime = new System.DateTime(1400, int.Parse(Request.Form["monthField"].ToString()), int.Parse(Request.Form["dayField"].ToString()), pc);

            _context.Shifts.Add(Shift);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
