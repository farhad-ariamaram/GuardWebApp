using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GuardWebApp.Models;

namespace GuardWebApp.Pages.CheckPage
{
    public class IndexModel : PageModel
    {
        private readonly GuardWebApp.Models.GuardianDBContext _context;

        public IndexModel(GuardWebApp.Models.GuardianDBContext context)
        {
            _context = context;
        }

        public IList<Check> Check { get;set; }

        public async Task OnGetAsync()
        {
            Check = await _context.Checks.ToListAsync();
        }
    }
}
