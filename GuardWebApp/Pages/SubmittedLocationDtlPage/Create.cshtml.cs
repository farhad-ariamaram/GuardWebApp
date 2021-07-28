using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GuardWebApp.Models;
using Microsoft.AspNetCore.Http;

namespace GuardWebApp.Pages.SubmittedLocationDtlPage
{
    public class CreateModel : PageModel
    {
        private readonly GuardWebApp.Models.GuardianDBContext _context;

        public CreateModel(GuardWebApp.Models.GuardianDBContext context)
        {
            _context = context;
        }

        public long LastId { get; set; }

        public IActionResult OnGet()
        {
            var uid = HttpContext.Session.GetString("uid");
            if (uid == null)
            {
                return RedirectToPage("../Index");
            }

            LastId = _context.SubmittedLocationDtls.OrderBy(a => a.Id).FirstOrDefault().Id;

            ViewData["LocationDetailId"] = new SelectList(_context.LocationDetails, "Id", "Id");
            ViewData["RunStatusId"] = new SelectList(_context.RunStatuses, "Id", "Name");
            ViewData["SubmittedLocationId"] = new SelectList(_context.SubmittedLocations, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public SubmittedLocationDtl SubmittedLocationDtl { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                LastId = _context.SubmittedLocationDtls.OrderBy(a => a.Id).FirstOrDefault().Id;
                return Page();
            }

            _context.SubmittedLocationDtls.Add(SubmittedLocationDtl);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
