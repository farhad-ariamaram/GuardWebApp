using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GuardWebApp.Models;
using Microsoft.AspNetCore.Http;

namespace GuardWebApp.Pages.RhythmDetailPage
{
    public class DeleteModel : PageModel
    {
        private readonly GuardWebApp.Models.GuardianDBContext _context;

        public DeleteModel(GuardWebApp.Models.GuardianDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public RhythmDetail RhythmDetail { get; set; }

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

            RhythmDetail = await _context.RhythmDetails
                .Include(r => r.Location)
                .Include(r => r.Rhythm).FirstOrDefaultAsync(m => m.Id == id);

            if (RhythmDetail == null)
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

            RhythmDetail = await _context.RhythmDetails.FindAsync(id);

            if (RhythmDetail != null)
            {
                _context.RhythmDetails.Remove(RhythmDetail);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index", new { rhythmId = RhythmDetail.RhythmId });
        }
    }
}
