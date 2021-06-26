using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GuardWebApp.Models;
using Microsoft.AspNetCore.Http;

namespace GuardWebApp.Pages.ClimatePage
{
    public class IndexModel : PageModel
    {
        private readonly GuardianDBContext _context;

        public IndexModel(GuardianDBContext context)
        {
            _context = context;
        }

        public IList<Climate> Climate { get;set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var uid = HttpContext.Session.GetString("uid");
            if (uid == null)
            {
                return RedirectToPage("../Index");
            }

            Climate = await _context.Climates.ToListAsync();

            return Page();
        }
    }
}
