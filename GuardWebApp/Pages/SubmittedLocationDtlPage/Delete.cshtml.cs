using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GuardWebApp.Models;
using Microsoft.AspNetCore.Http;

namespace GuardWebApp.Pages.SubmittedLocationDtlPage
{
    public class DeleteModel : PageModel
    {
        private readonly GuardWebApp.Models.GuardianDBContext _context;

        public DeleteModel(GuardWebApp.Models.GuardianDBContext context)
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
                .Include(s => s.SubmittedLocation).FirstOrDefaultAsync(m => m.Id == id);

            if (SubmittedLocationDtl == null)
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

            SubmittedLocationDtl = await _context.SubmittedLocationDtls.FindAsync(id);

            if (SubmittedLocationDtl != null)
            {
                _context.SubmittedLocationDtls.Remove(SubmittedLocationDtl);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
