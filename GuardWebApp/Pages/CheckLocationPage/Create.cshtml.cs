using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GuardWebApp.Models;
using GuardWebApp.ViewModels;

namespace GuardWebApp.Pages.CheckLocationPage
{
    public class CreateModel : PageModel
    {
        private readonly GuardWebApp.Models.GuardianDBContext _context;

        public CreateModel(GuardWebApp.Models.GuardianDBContext context)
        {
            _context = context;
        }

        public long locationIdProp { get; set; }

        [BindProperty]
        public MultiSelectionVM multiSelection { get; set; }

        public IActionResult OnGet(long? locationId)
        {
            if (!locationId.HasValue)
            {
                return RedirectToPage("../404");
            }

            var location = _context.Locations.Find(locationId);
            if (location == null)
            {
                return RedirectToPage("../404");
            }

            locationIdProp = locationId.Value;

            multiSelection = new MultiSelectionVM
            {
                SelectedIds = new long[] { },
                Items = _context.Visittimes.Select(a=> new SelectListItem { Value = a.Id.ToString() , Text = a.Title })
            };

            ViewData["CheckId"] = new SelectList(_context.Checks, "Id", "Name");
            ViewData["ClimateId"] = new SelectList(_context.Climates, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public CheckLocation CheckLocation { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.CheckLocations.Add(CheckLocation);
            await _context.SaveChangesAsync();

            try
            {
                foreach (var item in multiSelection.SelectedIds)
                {
                    _context.CheckLocationVisittimes.Add(new CheckLocationVisittime() { VisittimeId = item, CheckLocationId = CheckLocation.Id });
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

            }
            

            return RedirectToPage("./Index", new { locationId = CheckLocation.LocationId });
        }
    }
}
