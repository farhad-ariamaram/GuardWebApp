using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GuardWebApp.Models;

namespace GuardWebApp.Pages.PlanPage
{
    public class IndexModel : PageModel
    {
        private readonly GuardWebApp.Models.GuardianDBContext _context;

        public IndexModel(GuardWebApp.Models.GuardianDBContext context)
        {
            _context = context;
        }

        public IList<Plan> Plan { get;set; }

        public async Task OnGetAsync()
        {
            Plan = await _context.Plans
                .Include(p => p.Location)
                .Include(p => p.Shift)
                .Include(p => p.User).ToListAsync();
        }
    }
}
