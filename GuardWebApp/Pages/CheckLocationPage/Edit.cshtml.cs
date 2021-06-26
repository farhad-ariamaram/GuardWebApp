using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GuardWebApp.Models;
using GuardWebApp.ViewModels;
using Microsoft.AspNetCore.Http;

namespace GuardWebApp.Pages.CheckLocationPage
{
    public class EditModel : PageModel
    {
        private readonly GuardWebApp.Models.GuardianDBContext _context;

        public EditModel(GuardWebApp.Models.GuardianDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public CheckLocation CheckLocation { get; set; }

        [BindProperty]
        public MultiSelectionVM multiSelection { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            var uid = HttpContext.Session.GetString("uid");
            if (uid == null)
            {
                return RedirectToPage("../Index");
            }

            if (id == null)
            {
                return NotFound();
            }

            CheckLocation = await _context.CheckLocations
                .Include(c => c.Check)
                .Include(c => c.Climate)
                .Include(c => c.Location).FirstOrDefaultAsync(m => m.Id == id);

            if (CheckLocation == null)
            {
                return NotFound();
            }

            multiSelection = new MultiSelectionVM
            {
                SelectedIds = await _context.CheckLocationVisittimes.Where(a => a.CheckLocationId == id).Select(b => b.VisittimeId).ToArrayAsync(),
                Items = _context.Visittimes.Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Title })
            };

            ViewData["CheckId"] = new SelectList(_context.Checks, "Id", "Name");
            ViewData["ClimateId"] = new SelectList(_context.Climates, "Id", "Name");

            return Page();
        }

        public IActionResult OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(CheckLocation).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CheckLocationExists(CheckLocation.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            try
            {
                var foo = _context.CheckLocationVisittimes.Where(a => a.CheckLocationId == CheckLocation.Id);
                foreach (var item in foo)
                {
                    _context.Remove(item);
                }
                _context.SaveChanges();

                foreach (var item in multiSelection.SelectedIds)
                {
                    _context.CheckLocationVisittimes.Add(new CheckLocationVisittime() { CheckLocationId = CheckLocation.Id, VisittimeId = item });
                }
                _context.SaveChanges();
            }
            catch (Exception)
            {

            }
            

            return RedirectToPage("./Index", new { locationId = CheckLocation.LocationId });
        }

        private bool CheckLocationExists(long id)
        {
            return _context.CheckLocations.Any(e => e.Id == id);
        }
    }
}
