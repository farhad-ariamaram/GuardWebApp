using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GuardWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GuardWebApp.Pages.VisittimePage
{
    public class IndexModel : PageModel
    {
        private readonly GuardianDBContext _context;

        public IndexModel(GuardianDBContext context)
        {
            _context = context;
        }

        public IList<Visittime> Visittime { get;set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var uid = HttpContext.Session.GetString("uid");
            if (uid == null)
            {
                return RedirectToPage("../Index");
            }

            Visittime = await _context.Visittimes.ToListAsync();

            return Page();
        }
    }
}
