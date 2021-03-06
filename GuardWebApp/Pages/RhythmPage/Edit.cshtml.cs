using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GuardWebApp.Models;
using Microsoft.AspNetCore.Http;

namespace GuardWebApp.Pages.RhythmPage
{
    public class EditModel : PageModel
    {
        private readonly GuardWebApp.Models.GuardianDBContext _context;

        public EditModel(GuardWebApp.Models.GuardianDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Rhythm Rhythm { get; set; }

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

            Rhythm = await _context.Rhythms
                .Include(r => r.GuardArea).FirstOrDefaultAsync(m => m.Id == id);

            if (Rhythm == null)
            {
                return NotFound();
            }
           ViewData["GuardAreaId"] = new SelectList(_context.GuardAreas, "Id", "Description");
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

            _context.Attach(Rhythm).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RhythmExists(Rhythm.Id))
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

        private bool RhythmExists(long id)
        {
            return _context.Rhythms.Any(e => e.Id == id);
        }
    }
}
