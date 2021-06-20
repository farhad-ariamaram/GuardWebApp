using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GuardWebApp.Models;

namespace GuardWebApp.Pages.VisittimePage
{
    public class EditModel : PageModel
    {
        private readonly GuardWebApp.Models.GuardianDBContext _context;

        public EditModel(GuardWebApp.Models.GuardianDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Visittime Visittime { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Visittime = await _context.Visittimes.FirstOrDefaultAsync(m => m.Id == id);

            if (Visittime == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                Visittime.StartDate = new DateTime(2000, int.Parse(Request.Form["startdatemonth"].ToString()), int.Parse(Request.Form["startdateday"].ToString()));
            }
            catch (Exception)
            {
                ModelState.AddModelError("WrongStartDate", "فرمت وارد شده اشتباه است");
                return Page();
            }

            try
            {
                Visittime.EndDate = new DateTime(2000, int.Parse(Request.Form["enddatemonth"].ToString()), int.Parse(Request.Form["enddateday"].ToString()));
            }
            catch (Exception)
            {
                ModelState.AddModelError("WrongEndDate", "فرمت وارد شده اشتباه است");
                return Page();
            }

            _context.Attach(Visittime).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VisittimeExists(Visittime.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool VisittimeExists(long id)
        {
            return _context.Visittimes.Any(e => e.Id == id);
        }
    }
}
