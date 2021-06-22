using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GuardWebApp.Models;

namespace GuardWebApp.Pages.PlanPage
{
    public class DeleteModel : PageModel
    {
        private readonly GuardWebApp.Models.GuardianDBContext _context;

        public DeleteModel(GuardWebApp.Models.GuardianDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Plan Plan { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Plan = await _context.Plans.FindAsync(id);

            if (Plan != null)
            {
                _context.Plans.Remove(Plan);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
