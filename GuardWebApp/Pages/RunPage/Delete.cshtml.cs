using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GuardWebApp.Models;
using Microsoft.AspNetCore.Http;

namespace GuardWebApp.Pages.RunPage
{
    public class DeleteModel : PageModel
    {
        private readonly GuardWebApp.Models.GuardianDBContext _context;

        public DeleteModel(GuardWebApp.Models.GuardianDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Run Run { get; set; }

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

            Run = await _context.Runs
                .Include(r => r.Approver)
                .Include(r => r.Plan)
                .Include(r => r.Status)
                .Include(r => r.SubmittedLocation)
                .Include(r => r.User).FirstOrDefaultAsync(m => m.Id == id);

            if (Run == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Run = await _context.Runs.FindAsync(id);

            if (Run != null)
            {
                _context.Runs.Remove(Run);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
