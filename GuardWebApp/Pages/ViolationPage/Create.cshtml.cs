using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GuardWebApp.Models;
using Microsoft.AspNetCore.Http;

namespace GuardWebApp.Pages.ViolationPage
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

            ViewData["ApproverId"] = new SelectList(_context.Users, "Id", "Name");
            ViewData["RegistrarId"] = new SelectList(_context.Users, "Id", "Name");
            ViewData["RunId"] = new SelectList(_context.Runs, "Id", "Id");
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name");
            ViewData["ViolationTypeId"] = new SelectList(_context.ViolationTypes, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Violation Violation { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Violations.Add(Violation);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
