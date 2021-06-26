using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GuardWebApp.Pages.Planning
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            ViewData["step1"] = true;
            return Page();
        }

        public IActionResult OnPostDate(DateTime sDate, DateTime eDate)
        {
            ViewData["step2"] = true;
            return Page();
        }

        public IActionResult OnPostGuards(int guardId)
        {
            ViewData["step3"] = true;
            return Page();
        }
    }
}
