using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GuardWebApp.Models;
using Microsoft.AspNetCore.Http;

namespace GuardWebApp.Pages.CheckLocationPage
{
    public class DeleteModel : PageModel
    {
        private readonly GuardWebApp.Models.GuardianDBContext _context;

        public DeleteModel(GuardWebApp.Models.GuardianDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public CheckLocation CheckLocation { get; set; }

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

            CheckLocation = await _context.CheckLocations
                .Include(c => c.Check)
                .Include(c => c.Climate)
                .Include(c => c.Location)
                .Include(c => c.CheckLocationVisittimes).ThenInclude(a => a.Visittime).FirstOrDefaultAsync(m => m.Id == id);

            if (CheckLocation == null)
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

            CheckLocation = await _context.CheckLocations.FindAsync(id);

            if (CheckLocation != null)
            {
                _context.CheckLocations.Remove(CheckLocation);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index", new { locationId = CheckLocation.LocationId });
        }
    }
}
