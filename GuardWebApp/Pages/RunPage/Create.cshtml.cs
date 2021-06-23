using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GuardWebApp.Models;
using Microsoft.AspNetCore.Http;

namespace GuardWebApp.Pages.RunPage
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

            ViewData["ApproverId"] = new SelectList(_context.Users, "Id", "Name");
            ViewData["PlanId"] = new SelectList(_context.Plans, "Id", "Id");
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Name");
            ViewData["SubmittedLocationId"] = new SelectList(_context.SubmittedLocations, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Run Run { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Runs.Add(Run);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
