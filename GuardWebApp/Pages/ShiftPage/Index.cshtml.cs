using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GuardWebApp.Models;
using Microsoft.AspNetCore.Http;
using System;
using GuardWebApp.Utilities;
using System.Globalization;
using System.Linq;

namespace GuardWebApp.Pages.ShiftPage
{
    public class IndexModel : PageModel
    {
        private readonly GuardWebApp.Models.GuardianDBContext _context;

        public IndexModel(GuardWebApp.Models.GuardianDBContext context)
        {
            _context = context;
        }

        public IList<Shift> Shift { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var uid = HttpContext.Session.GetString("uid");
            if (uid == null)
            {
                return RedirectToPage("../Index");
            }

            Shift = await _context.Shifts
                .Include(s => s.GuardArea)
                .Include(s => s.Rhythm).ToListAsync();

            return Page();
        }

        public IActionResult OnPostAuto(string sdayField, string smonthField, string edayField, string emonthField)
        {
            PersianCalendar pc = new PersianCalendar();

            var guardAreasList = _context.GuardAreas.ToList();

            foreach (DateTime day in Utils.EachDay(new DateTime(1400, int.Parse(smonthField), int.Parse(sdayField), pc), new DateTime(1400, int.Parse(emonthField), int.Parse(edayField), pc)))
            {
                foreach (var item in guardAreasList)
                {
                    var rhytms = _context.Rhythms.Where(a => a.GuardAreaId == item.Id).ToList();
                    if (rhytms.Any())
                    {
                        Random rnd = new Random();
                        int randomRythm = rnd.Next(rhytms.Count);
                        Models.Shift shift = new Models.Shift()
                        {
                            DateTime = day,
                            GuardAreaId = item.Id,
                            RhythmId = rhytms[randomRythm].Id
                        };
                        _context.Shifts.Add(shift);
                        
                    }
                }
            }
            _context.SaveChanges();

            Shift = _context.Shifts
                .Include(s => s.GuardArea)
                .Include(s => s.Rhythm).ToList();

            return Page();
        }
    }
}
