using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GuardWebApp.Models;
using Microsoft.AspNetCore.Http;

namespace GuardWebApp.Pages.RunDetailPage
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

            ViewData["LocationDetailId"] = new SelectList(_context.LocationDetails, "Id", "Id");
            ViewData["RunId"] = new SelectList(_context.Runs, "Id", "Id");
            ViewData["RunStatusId"] = new SelectList(_context.RunStatuses, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public RunDetail RunDetail { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.RunDetails.Add(RunDetail);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
