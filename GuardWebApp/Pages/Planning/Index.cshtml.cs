using System;
using GuardWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GuardWebApp.Pages.Planning
{
    public class IndexModel : PageModel
    {
        private readonly GuardianDBContext _context;

        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }

        public IndexModel(GuardianDBContext context)
        {
            _context = context;
        }


        public IActionResult OnGet()
        {
            ViewData["step1"] = true;
            return Page();
        }

        public IActionResult OnPostDate(string sDate, string eDate, string timetype)
        {
            switch (timetype)
            {
                case "tofrom":
                    startDate = Convert.ToDateTime(sDate);
                    endDate = Convert.ToDateTime(eDate);
                    break;
                case "daily":
                    startDate = Convert.ToDateTime(sDate);
                    endDate = startDate;
                    break;
                case "monthly":
                    startDate = new DateTime(2000,int.Parse(sDate),1);
                    endDate = new DateTime(2000, int.Parse(sDate), DateTime.DaysInMonth(2000, int.Parse(sDate)));
                    break;
                default:
                    break;
            }

            ViewData["step2"] = true;
            ViewData["GuardsId"] = new SelectList(_context.Users, "Id", "Name");
            return Page();
        }

        public IActionResult OnPostGuards(int guardId)
        {
            ViewData["step3"] = true;
            ViewData["GuardAreasId"] = new SelectList(_context.GuardAreas, "Id", "Description");
            return Page();
        }

        public IActionResult OnPostGuardAreas(int guardAreaId)
        {
            ViewData["step4"] = true;
            ViewData["ShiftsId"] = new SelectList(_context.Shifts, "Id", "DateTime");
            return Page();
        }

        public IActionResult OnPostShifts(int shiftId)
        {
            ViewData["step5"] = true;
            ViewData["LocationsId"] = new SelectList(_context.Locations, "Id", "Name");
            return Page();
        }

        public IActionResult OnPostLocations(int locationId)
        {
            ViewData["step6"] = true;
            ViewData["RhythmsId"] = new SelectList(_context.Rhythms, "Id", "Title");
            return Page();
        }

        public IActionResult OnPostRhythms(int rhythmId)
        {
            ViewData["step7"] = true;
            ViewData["RhythmDetailsId"] = new SelectList(_context.RhythmDetails, "Id", "Time");
            return Page();
        }

        public IActionResult OnPostRhythmDetails(int rhythmDetailId)
        {
            ViewData["step8"] = true;
            return Page();
        }
    }
}
