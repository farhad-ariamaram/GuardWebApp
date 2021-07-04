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
    public class IndexModel : PageModel
    {
        private readonly GuardWebApp.Models.GuardianDBContext _context;

        public IndexModel(GuardWebApp.Models.GuardianDBContext context)
        {
            _context = context;
        }

        public IList<RhythmDetail> RhythmDetail { get;set; }

        public long rhythmIdProp { get; set; }

        public async Task<IActionResult> OnGetAsync(long? rhythmId)
        {
            var uid = HttpContext.Session.GetString("uid");
            if (uid == null)
            {
                return RedirectToPage("../Index");
            }

            if (!rhythmId.HasValue)
            {
                return RedirectToPage("../404");
            }

            var rhythm = await _context.Rhythms.FindAsync(rhythmId);
            if (rhythm == null)
            {
                return RedirectToPage("../404");
            }

            rhythmIdProp = rhythmId.Value;

            ViewData["CurrentRhythm"] = rhythm.Title;

            RhythmDetail = await _context.RhythmDetails
                .Where(a => a.RhythmId == rhythmId)
                .Include(r => r.Location)
                .Include(r => r.Rhythm).ToListAsync();

            return Page();
        }
    }
}
