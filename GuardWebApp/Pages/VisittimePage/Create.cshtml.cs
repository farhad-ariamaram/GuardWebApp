using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GuardWebApp.Models;
using Microsoft.AspNetCore.Http;
using System.Globalization;

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

            PersianCalendar pc = new PersianCalendar();

            try
            {
                if (Request.Form["startdatemonth"].ToString() == "" && Request.Form["startdateday"].ToString() == "")
                {
                    Visittime.StartDate = new DateTime(2000, 3, 20);
                }
                else
                {
                    DateTime d = new DateTime(1379, int.Parse(Request.Form["startdatemonth"].ToString()), int.Parse(Request.Form["startdateday"].ToString()), pc);
                    Visittime.StartDate = d;
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("WrongStartDate", "فرمت وارد شده اشتباه است");
                return Page();
            }

            try
            {
                if (Request.Form["enddatemonth"].ToString() == "" && Request.Form["enddateday"].ToString() == "")
                {
                    Visittime.EndDate = new DateTime(2000, 3, 19);
                }
                else
                {
                    DateTime d = new DateTime(1379, int.Parse(Request.Form["enddatemonth"].ToString()), int.Parse(Request.Form["enddateday"].ToString()),pc);
                    Visittime.EndDate = d;
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("WrongEndDate", "فرمت وارد شده اشتباه است");
                return Page();
            }

            try
            {
                if (Request.Form["Visittime.StartTime"].ToString() == "")
                {
                    Visittime.StartTime = new TimeSpan(0, 0, 0);
                }
            }
            catch (Exception)
            {
                Visittime.StartTime = new TimeSpan(0, 0, 0);
            }

            try
            {
                if (Request.Form["Visittime.EndTime"].ToString() == "")
                {
                    Visittime.EndTime = new TimeSpan(23, 59, 59);
                }
            }
            catch (Exception)
            {
                Visittime.EndTime = new TimeSpan(23, 59, 59);
            }

            _context.Visittimes.Add(Visittime);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
