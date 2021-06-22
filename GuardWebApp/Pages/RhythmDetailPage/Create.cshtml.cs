using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GuardWebApp.Models;

namespace GuardWebApp.Pages.RhythmDetailPage
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
        ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Name");
        ViewData["RhythmId"] = new SelectList(_context.Rhythms, "Id", "Title");
            return Page();
        }

        [BindProperty]
        public RhythmDetail RhythmDetail { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.RhythmDetails.Add(RhythmDetail);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
