using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GuardWebApp.Models;

namespace GuardWebApp.Pages.PlanPage
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
        ViewData["ShiftId"] = new SelectList(_context.Shifts, "Id", "Id");
        ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Plan Plan { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Plans.Add(Plan);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
