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

namespace GuardWebApp.Pages.SubmittedLocationPage
{
    public class EditModel : PageModel
    {
        private readonly GuardWebApp.Models.GuardianDBContext _context;

        public EditModel(GuardWebApp.Models.GuardianDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SubmittedLocation SubmittedLocation { get; set; }

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

            SubmittedLocation = await _context.SubmittedLocations
                .Include(s => s.Device)
                .Include(s => s.Location)
                .Include(s => s.User).FirstOrDefaultAsync(m => m.Id == id);

            if (SubmittedLocation == null)
            {
                return NotFound();
            }
           ViewData["DeviceId"] = new SelectList(_context.Devices, "Id", "Name");
           ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Name");
           ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(SubmittedLocation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubmittedLocationExists(SubmittedLocation.Id))
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

        private bool SubmittedLocationExists(long id)
        {
            return _context.SubmittedLocations.Any(e => e.Id == id);
        }
    }
}
