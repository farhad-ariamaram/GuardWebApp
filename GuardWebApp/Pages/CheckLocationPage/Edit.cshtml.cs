using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GuardWebApp.Models;

namespace GuardWebApp.Pages.CheckLocationPage
{
    public class EditModel : PageModel
    {
        private readonly GuardWebApp.Models.GuardianDBContext _context;

        public EditModel(GuardWebApp.Models.GuardianDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public CheckLocation CheckLocation { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CheckLocation = await _context.CheckLocations
                .Include(c => c.Check)
                .Include(c => c.Climate)
                .Include(c => c.Location).FirstOrDefaultAsync(m => m.Id == id);

            if (CheckLocation == null)
            {
                return NotFound();
            }

            ViewData["CheckId"] = new SelectList(_context.Checks, "Id", "Name");
            ViewData["ClimateId"] = new SelectList(_context.Climates, "Id", "Name");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(CheckLocation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CheckLocationExists(CheckLocation.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index", new { locationId = CheckLocation.LocationId });
        }

        private bool CheckLocationExists(long id)
        {
            return _context.CheckLocations.Any(e => e.Id == id);
        }
    }
}
