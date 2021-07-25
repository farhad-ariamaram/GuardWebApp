using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GuardWebApp.Models;
using Microsoft.AspNetCore.Http;

namespace GuardWebApp.Pages.LocationDetailPage
{
    public class CreateModel : PageModel
    {
        private readonly GuardWebApp.Models.GuardianDBContext _context;

        public CreateModel(GuardWebApp.Models.GuardianDBContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            var uid = HttpContext.Session.GetString("uid");
            if (uid == null)
            {
                return RedirectToPage("../Index");
            }

            ViewData["CheckId"] = new SelectList(_context.Checks, "Id", "Name");
            ViewData["ClimateId"] = new SelectList(_context.Climates, "Id", "Name");
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Name");

            return Page();
        }

        [BindProperty]
        public LocationDetail LocationDetail { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.LocationDetails.Add(LocationDetail);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
