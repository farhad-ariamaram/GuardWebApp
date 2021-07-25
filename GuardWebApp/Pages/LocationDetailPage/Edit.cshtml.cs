using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GuardWebApp.Models;
using Microsoft.AspNetCore.Http;

namespace GuardWebApp.Pages.LocationDetailPage
{
    public class EditModel : PageModel
    {
        private readonly GuardWebApp.Models.GuardianDBContext _context;

        public EditModel(GuardWebApp.Models.GuardianDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public LocationDetail LocationDetail { get; set; }

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

            LocationDetail = await _context.LocationDetails
                .Include(l => l.Check)
                .Include(l => l.Climate)
                .Include(l => l.Location).FirstOrDefaultAsync(m => m.Id == id);

            if (LocationDetail == null)
            {
                return NotFound();
            }

            ViewData["CheckId"] = new SelectList(_context.Checks, "Id", "Name");
            ViewData["ClimateId"] = new SelectList(_context.Climates, "Id", "Name");
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Name");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(LocationDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationDetailExists(LocationDetail.Id))
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

        private bool LocationDetailExists(long id)
        {
            return _context.LocationDetails.Any(e => e.Id == id);
        }
    }
}
