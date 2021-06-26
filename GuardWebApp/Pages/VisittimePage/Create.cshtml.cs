using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GuardWebApp.Models;
using Microsoft.AspNetCore.Http;

namespace GuardWebApp.Pages.VisittimePage
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

            return Page();
        }

        [BindProperty]
        public Visittime Visittime { get; set; }

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

            _context.Visittimes.Add(Visittime);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
