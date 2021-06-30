using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GuardWebApp.Models;
using Microsoft.AspNetCore.Http;
using GuardWebApp.Utilities;
using System.Globalization;

namespace GuardWebApp.Pages.GuardAreaAllocationPage
{
    public class EditModel : PageModel
    {
        private readonly GuardWebApp.Models.GuardianDBContext _context;

        public EditModel(GuardWebApp.Models.GuardianDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public GuardAreaAllocation GuardAreaAllocation { get; set; }

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

            GuardAreaAllocation = await _context.GuardAreaAllocations
                .Include(g => g.GuardArea)
                .Include(g => g.User).FirstOrDefaultAsync(m => m.Id == id);

            if (GuardAreaAllocation == null)
            {
                return NotFound();
            }
            ViewData["GuardAreaId"] = new SelectList(_context.GuardAreas, "Id", "Description");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name");
            return Page();
        }

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
                GuardAreaAllocation.EndDate = new DateTime(1400, 12, 29, pc);
            }
            else
            {
                GuardAreaAllocation.EndDate = new System.DateTime(1400, int.Parse(Request.Form["emonthField"].ToString()), int.Parse(Request.Form["edayField"].ToString()), pc);
            }

            _context.Attach(GuardAreaAllocation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                var shiftAllocation = await _context.ShiftAllocations.Where(a => a.GuardAreaId == GuardAreaAllocation.GuardAreaId && a.UserId == GuardAreaAllocation.UserId).ToListAsync();
                foreach (var item in shiftAllocation)
                {
                    _context.ShiftAllocations.Remove(item);
                }
                await _context.SaveChangesAsync();

                foreach (DateTime day in Utils.EachDay(GuardAreaAllocation.StartDate, (DateTime)GuardAreaAllocation.EndDate))
                {
                    var shift = _context.Shifts.FirstOrDefault(a => a.GuardAreaId == GuardAreaAllocation.GuardAreaId && a.DateTime == day);
                    if (shift != null)
                    {
                        ShiftAllocation shiftAllocation1 = new ShiftAllocation
                        {
                            DateTime = day,
                            GuardAreaId = GuardAreaAllocation.GuardAreaId,
                            RhythmId = shift.RhythmId,
                            UserId = GuardAreaAllocation.UserId
                        };

                        if (!_context.ShiftAllocations.Where(a => a.DateTime == day && a.GuardAreaId == GuardAreaAllocation.GuardAreaId && a.RhythmId == shift.RhythmId && a.UserId == GuardAreaAllocation.UserId).Any())
                        {
                            await _context.ShiftAllocations.AddAsync(shiftAllocation1);
                            await _context.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        ViewData["noShiftsDates"] += "," + day.ToString();
                    }
                }

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GuardAreaAllocationExists(GuardAreaAllocation.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index", new { nf = ViewData["noShiftsDates"] });
        }

        private bool GuardAreaAllocationExists(long id)
        {
            return _context.GuardAreaAllocations.Any(e => e.Id == id);
        }
    }
}
