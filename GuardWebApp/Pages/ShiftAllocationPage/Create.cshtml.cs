using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GuardWebApp.Models;
using Microsoft.AspNetCore.Http;

namespace GuardWebApp.Pages.ShiftAllocationPage
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public ShiftAllocation ShiftAllocation { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.ShiftAllocations.Add(ShiftAllocation);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
