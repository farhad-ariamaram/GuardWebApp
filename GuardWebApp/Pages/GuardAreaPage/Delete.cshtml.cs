using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GuardWebApp.Models;

namespace GuardWebApp.Pages.GuardAreaPage
{
    public class DeleteModel : PageModel
    {
        private readonly GuardWebApp.Models.GuardianDBContext _context;

        public DeleteModel(GuardWebApp.Models.GuardianDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public GuardArea GuardArea { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            GuardArea = await _context.GuardAreas.FirstOrDefaultAsync(m => m.Id == id);

            if (GuardArea == null)
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

            GuardArea = await _context.GuardAreas.FindAsync(id);

            if (GuardArea != null)
            {
                _context.GuardAreas.Remove(GuardArea);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
