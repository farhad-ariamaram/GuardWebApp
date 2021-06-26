using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GuardWebApp.Models;
using Microsoft.AspNetCore.Http;

namespace GuardWebApp.Pages.LocationDetailPage
{
    public class IndexModel : PageModel
    {
        private readonly GuardWebApp.Models.GuardianDBContext _context;

        public IndexModel(GuardWebApp.Models.GuardianDBContext context)
        {
            _context = context;
        }

        public IList<LocationDetail> LocationDetail { get;set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var uid = HttpContext.Session.GetString("uid");
            if (uid == null)
            {
                return RedirectToPage("../Index");
            }

            LocationDetail = await _context.LocationDetails
                .Include(l => l.CheckLocation)
                .Include(l => l.Climate)
                .Include(l => l.Location).ToListAsync();

            return Page();
        }
    }
}
