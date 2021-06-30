using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GuardWebApp.Models;
using Microsoft.AspNetCore.Http;
using GuardWebApp.Utilities;
using System.Globalization;

namespace GuardWebApp.Pages.GuardAreaAllocationPage
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

            ViewData["GuardAreaId"] = new SelectList(_context.GuardAreas, "Id", "Description");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public GuardAreaAllocation GuardAreaAllocation { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            PersianCalendar pc = new PersianCalendar();
            GuardAreaAllocation.StartDate = new System.DateTime(1400, int.Parse(Request.Form["smonthField"].ToString()), int.Parse(Request.Form["sdayField"].ToString()), pc);

            if (string.IsNullOrEmpty(Request.Form["emonthField"].ToString()) || string.IsNullOrEmpty(Request.Form["edayField"].ToString()))
            {
                GuardAreaAllocation.EndDate = new DateTime(1400,12,29,pc);
            }
            else
            {
                GuardAreaAllocation.EndDate = new System.DateTime(1400, int.Parse(Request.Form["emonthField"].ToString()), int.Parse(Request.Form["edayField"].ToString()), pc);
            }

            await _context.GuardAreaAllocations.AddAsync(GuardAreaAllocation);
            await _context.SaveChangesAsync();

            foreach (DateTime day in Utils.EachDay(GuardAreaAllocation.StartDate, (DateTime)GuardAreaAllocation.EndDate))
            {
                var shift = _context.Shifts.FirstOrDefault(a => a.GuardAreaId == GuardAreaAllocation.GuardAreaId && a.DateTime == day);
                if(shift != null)
                {
                    ShiftAllocation shiftAllocation = new ShiftAllocation
                    {
                        DateTime = day,
                        GuardAreaId = GuardAreaAllocation.GuardAreaId,
                        RhythmId = shift.RhythmId,
                        UserId = GuardAreaAllocation.UserId
                    };

                    if (!_context.ShiftAllocations.Where(a=>a.DateTime==day && a.GuardAreaId== GuardAreaAllocation.GuardAreaId && a.RhythmId== shift.RhythmId &&a.UserId== GuardAreaAllocation.UserId).Any())
                    {
                        await _context.ShiftAllocations.AddAsync(shiftAllocation);
                        await _context.SaveChangesAsync();
                    }
                }
                else
                {
                    ViewData["noShiftsDates"] += "," + day.ToString();
                }
            }

            return RedirectToPage("./Index", new { nf = ViewData["noShiftsDates"] });
        }
    }
}
