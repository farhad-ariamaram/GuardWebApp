using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GuardWebApp.Models;

namespace GuardWebApp.Pages.CheckPage
{
    public class DeleteModel : PageModel
    {
        private readonly GuardWebApp.Models.GuardianDBContext _context;

        public DeleteModel(GuardWebApp.Models.GuardianDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Check Check { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Check = await _context.Checks.FirstOrDefaultAsync(m => m.Id == id);

            if (Check == null)
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

            Check = await _context.Checks.FindAsync(id);

            if (Check != null)
            {
                _context.Checks.Remove(Check);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
