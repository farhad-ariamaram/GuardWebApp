using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GuardWebApp.Models;

namespace GuardWebApp.Pages.ShiftAllocationPage
{
    public class IndexModel : PageModel
    {
        private readonly GuardWebApp.Models.GuardianDBContext _context;

        public IndexModel(GuardWebApp.Models.GuardianDBContext context)
        {
            _context = context;
        }

        public IList<ShiftAllocation> ShiftAllocation { get;set; }

        public async Task OnGetAsync()
        {
            ShiftAllocation = await _context.ShiftAllocations
                .Include(s => s.GuardArea)
                .Include(s => s.Rhythm).ToListAsync();
        }
    }
}
