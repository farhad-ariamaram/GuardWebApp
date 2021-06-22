using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GuardWebApp.Models;

namespace GuardWebApp.Pages.ShiftAllocationPage
{
    public class EditModel : PageModel
    {
        private readonly GuardWebApp.Models.GuardianDBContext _context;

        public EditModel(GuardWebApp.Models.GuardianDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ShiftAllocation ShiftAllocation { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ShiftAllocation = await _context.ShiftAllocations
                .Include(s => s.GuardArea)
                .Include(s => s.Rhythm).FirstOrDefaultAsync(m => m.Id == id);

            if (ShiftAllocation == null)
            {
                return NotFound();
            }
           ViewData["GuardAreaId"] = new SelectList(_context.GuardAreas, "Id", "Description");
           ViewData["RhythmId"] = new SelectList(_context.Rhythms, "Id", "Title");
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

            _context.Attach(ShiftAllocation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShiftAllocationExists(ShiftAllocation.Id))
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

        private bool ShiftAllocationExists(long id)
        {
            return _context.ShiftAllocations.Any(e => e.Id == id);
        }
    }
}
