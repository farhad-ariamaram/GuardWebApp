using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GuardWebApp.Models;

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

        public async Task OnGetAsync()
        {
            Visittime = await _context.Visittimes.ToListAsync();
        }
    }
}
