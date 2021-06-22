using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GuardWebApp.Models;

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

        public async Task OnGetAsync()
        {
            RhythmDetail = await _context.RhythmDetails
                .Include(r => r.Location)
                .Include(r => r.Rhythm).ToListAsync();
        }
    }
}
