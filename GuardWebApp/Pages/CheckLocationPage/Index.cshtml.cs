using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GuardWebApp.Models;
using Microsoft.AspNetCore.Http;

namespace GuardWebApp.Pages.CheckLocationPage
{
    public class IndexModel : PageModel
    {
        private readonly GuardWebApp.Models.GuardianDBContext _context;

        public IndexModel(GuardWebApp.Models.GuardianDBContext context)
        {
            _context = context;
        }

        public IList<CheckLocation> CheckLocation { get; set; }

        public long locationIdProp { get; set; }

        public async Task<IActionResult> OnGetAsync(long? locationId)
        {
            var uid = HttpContext.Session.GetString("uid");
            if (uid == null)
            {
                return RedirectToPage("../Index");
            }

            if (!locationId.HasValue)
            {
                return RedirectToPage("../404");
            }

            var location = await _context.Locations.FindAsync(locationId);
            if (location == null)
            {
                return RedirectToPage("../404");
            }

            locationIdProp = locationId.Value;

            CheckLocation = await _context.CheckLocations
                .Where(a => a.LocationId == locationId)
                .Include(c => c.Check)
                .Include(c => c.Climate)
                .Include(c => c.Location)
                .Include(c => c.CheckLocationVisittimes).ThenInclude(a=>a.Visittime).ToListAsync();

            return Page();
        }
    }
}
