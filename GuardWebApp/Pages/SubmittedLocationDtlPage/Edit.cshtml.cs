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

namespace GuardWebApp.Pages.SubmittedLocationDtlPage
{
    public class EditModel : PageModel
    {
        private readonly GuardWebApp.Models.GuardianDBContext _context;

        public EditModel(GuardWebApp.Models.GuardianDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SubmittedLocationDtl SubmittedLocationDtl { get; set; }

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

            SubmittedLocationDtl = await _context.SubmittedLocationDtls
                .Include(s => s.LocationDetail)
                .Include(s => s.RunStatus)
                .Include(s => s.SubmittedLocation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (SubmittedLocationDtl == null)
            {
                return NotFound();
            }
           ViewData["LocationDetailId"] = new SelectList(_context.LocationDetails, "Id", "Id");
           ViewData["RunStatusId"] = new SelectList(_context.RunStatuses, "Id", "Name");
           ViewData["SubmittedLocationId"] = new SelectList(_context.SubmittedLocations, "Id", "Id");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(SubmittedLocationDtl).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubmittedLocationDtlExists(SubmittedLocationDtl.Id))
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

        private bool SubmittedLocationDtlExists(long id)
        {
            return _context.SubmittedLocationDtls.Any(e => e.Id == id);
        }
    }
}
