using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GuardWebApp.Models;
using Microsoft.AspNetCore.Http;

namespace GuardWebApp.Pages.RhythmDetailPage
{
    public class EditModel : PageModel
    {
        private readonly GuardWebApp.Models.GuardianDBContext _context;

        public EditModel(GuardWebApp.Models.GuardianDBContext context)
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
           ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Name");
           ViewData["RhythmId"] = new SelectList(_context.Rhythms, "Id", "Title");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(RhythmDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RhythmDetailExists(RhythmDetail.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index", new { rhythmId = RhythmDetail.RhythmId });
        }

        private bool RhythmDetailExists(long id)
        {
            return _context.RhythmDetails.Any(e => e.Id == id);
        }
    }
}
