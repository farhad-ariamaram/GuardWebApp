using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GuardWebApp.Models;
using Microsoft.AspNetCore.Http;
using System.Globalization;

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
            var uid = HttpContext.Session.GetString("uid");
            if (uid == null)
            {
                return RedirectToPage("../Index");
            }

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
                    DateTime d = new DateTime(1379, int.Parse(Request.Form["enddatemonth"].ToString()), int.Parse(Request.Form["enddateday"].ToString()), pc);
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

            //try
            //{
            //    Visittime.StartDate = new DateTime(2000, int.Parse(Request.Form["startdatemonth"].ToString()), int.Parse(Request.Form["startdateday"].ToString()));
            //}
            //catch (Exception)
            //{
            //    ModelState.AddModelError("WrongStartDate", "فرمت وارد شده اشتباه است");
            //    return Page();
            //}

            //try
            //{
            //    Visittime.EndDate = new DateTime(2000, int.Parse(Request.Form["enddatemonth"].ToString()), int.Parse(Request.Form["enddateday"].ToString()));
            //}
            //catch (Exception)
            //{
            //    ModelState.AddModelError("WrongEndDate", "فرمت وارد شده اشتباه است");
            //    return Page();
            //}

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
