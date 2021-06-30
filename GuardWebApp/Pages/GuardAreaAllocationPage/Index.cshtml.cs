using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GuardWebApp.Models;
using Microsoft.AspNetCore.Http;

namespace GuardWebApp.Pages.GuardAreaAllocationPage
{
    public class IndexModel : PageModel
    {
        private readonly GuardWebApp.Models.GuardianDBContext _context;

        public IndexModel(GuardWebApp.Models.GuardianDBContext context)
        {
            _context = context;
        }

        public IList<GuardAreaAllocation> GuardAreaAllocation { get;set; }

        public string[] notFillDates { get; set; }

        public async Task<IActionResult> OnGetAsync(string nf)
        {
            var uid = HttpContext.Session.GetString("uid");
            if (uid == null)
            {
                return RedirectToPage("../Index");
            }

            GuardAreaAllocation = await _context.GuardAreaAllocations
                .Include(g => g.GuardArea)
                .Include(g => g.User).ToListAsync();

            if (!string.IsNullOrEmpty(nf))
            {
                notFillDates = nf.Split(',');
            }

            return Page();
        }
    }
}
