using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GuardWebApp.Models;
using Microsoft.AspNetCore.Http;

namespace GuardWebApp.Pages.RunPage
{
    public class EditModel : PageModel
    {
        private readonly GuardWebApp.Models.GuardianDBContext _context;

        public EditModel(GuardWebApp.Models.GuardianDBContext context)
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
           ViewData["ApproverId"] = new SelectList(_context.Users, "Id", "Name");
           ViewData["PlanId"] = new SelectList(_context.Plans, "Id", "Id");
           ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Name");
           ViewData["SubmittedLocationId"] = new SelectList(_context.SubmittedLocations, "Id", "Id");
           ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Run).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RunExists(Run.Id))
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

        private bool RunExists(long id)
        {
            return _context.Runs.Any(e => e.Id == id);
        }
    }
}
