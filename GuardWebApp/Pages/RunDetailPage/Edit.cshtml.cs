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

namespace GuardWebApp.Pages.RunDetailPage
{
    public class EditModel : PageModel
    {
        private readonly GuardWebApp.Models.GuardianDBContext _context;

        public EditModel(GuardWebApp.Models.GuardianDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public RunDetail RunDetail { get; set; }

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

            RunDetail = await _context.RunDetails
                .Include(r => r.LocationDetail)
                .Include(r => r.Run)
                .Include(r => r.RunStatus).FirstOrDefaultAsync(m => m.Id == id);

            if (RunDetail == null)
            {
                return NotFound();
            }
           ViewData["LocationDetailId"] = new SelectList(_context.LocationDetails, "Id", "Id");
           ViewData["RunId"] = new SelectList(_context.Runs, "Id", "Id");
           ViewData["RunStatusId"] = new SelectList(_context.RunStatuses, "Id", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(RunDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RunDetailExists(RunDetail.Id))
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

        private bool RunDetailExists(long id)
        {
            return _context.RunDetails.Any(e => e.Id == id);
        }
    }
}
