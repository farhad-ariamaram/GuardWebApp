using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GuardWebApp.Models;
using Microsoft.AspNetCore.Http;

namespace GuardWebApp.Pages.ViolationPage
{
    public class EditModel : PageModel
    {
        private readonly GuardWebApp.Models.GuardianDBContext _context;

        public EditModel(GuardWebApp.Models.GuardianDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Violation Violation { get; set; }

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

            Violation = await _context.Violations
                .Include(v => v.Approver)
                .Include(v => v.Registrar)
                .Include(v => v.Run)
                .Include(v => v.Status)
                .Include(v => v.User)
                .Include(v => v.ViolationType).FirstOrDefaultAsync(m => m.Id == id);

            if (Violation == null)
            {
                return NotFound();
            }
           ViewData["ApproverId"] = new SelectList(_context.Users, "Id", "Name");
           ViewData["RegistrarId"] = new SelectList(_context.Users, "Id", "Name");
           ViewData["RunId"] = new SelectList(_context.Runs, "Id", "Id");
           ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Name");
           ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name");
           ViewData["ViolationTypeId"] = new SelectList(_context.ViolationTypes, "Id", "Name");
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

            _context.Attach(Violation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ViolationExists(Violation.Id))
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

        private bool ViolationExists(long id)
        {
            return _context.Violations.Any(e => e.Id == id);
        }
    }
}
