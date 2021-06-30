using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GuardWebApp.Models;
using Microsoft.AspNetCore.Http;

namespace GuardWebApp.Pages.RhythmDetailPage
{
    public class CreateModel : PageModel
    {
        private readonly GuardWebApp.Models.GuardianDBContext _context;

        public CreateModel(GuardWebApp.Models.GuardianDBContext context)
        {
            _context = context;
        }

        public long rhythmIdProp { get; set; }

        public IActionResult OnGet(long? rhythmId)
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

            var rythm = _context.Rhythms.Find(rhythmId);
            if (rythm == null)
            {
                return RedirectToPage("../404");
            }

            rhythmIdProp = rhythmId.Value;

            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Name");
            ViewData["RhythmId"] = new SelectList(_context.Rhythms, "Id", "Title");
            return Page();
        }

        [BindProperty]
        public RhythmDetail RhythmDetail { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.RhythmDetails.Add(RhythmDetail);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index", new {rhythmId = RhythmDetail.RhythmId });
        }
    }
}
