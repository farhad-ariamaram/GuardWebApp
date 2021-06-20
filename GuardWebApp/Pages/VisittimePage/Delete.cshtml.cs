using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GuardWebApp.Models;

namespace GuardWebApp.Pages.VisittimePage
{
    public class DeleteModel : PageModel
    {
        private readonly GuardWebApp.Models.GuardianDBContext _context;

        public DeleteModel(GuardWebApp.Models.GuardianDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Visittime Visittime { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Visittime = await _context.Visittimes.FirstOrDefaultAsync(m => m.Id == id);

            if (Visittime == null)
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

            Visittime = await _context.Visittimes.FindAsync(id);

            if (Visittime != null)
            {
                _context.Visittimes.Remove(Visittime);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
