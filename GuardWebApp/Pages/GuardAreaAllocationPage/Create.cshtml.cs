﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GuardWebApp.Models;
using Microsoft.AspNetCore.Http;
using GuardWebApp.Utilities;

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

            if (GuardAreaAllocation.EndDate == null)
            {
                GuardAreaAllocation.EndDate = new DateTime(GuardAreaAllocation.StartDate.Year,12,29);
            }

            await _context.GuardAreaAllocations.AddAsync(GuardAreaAllocation);
            await _context.SaveChangesAsync();

            foreach (DateTime day in Utils.EachDay(GuardAreaAllocation.StartDate, (DateTime)GuardAreaAllocation.EndDate))
            {
                if(_context.Shifts.FirstOrDefault(a => a.GuardAreaId == GuardAreaAllocation.GuardAreaId && a.DateTime.Month == day.Month && a.DateTime.Day == day.Day) != null)
                {
                    long rhythmId = _context.Shifts.FirstOrDefault(a => a.GuardAreaId == GuardAreaAllocation.GuardAreaId && a.DateTime.Month == day.Month && a.DateTime.Day == day.Day).RhythmId;
                    ShiftAllocation shiftAllocation = new ShiftAllocation
                    {
                        DateTime = day,
                        GuardAreaId = GuardAreaAllocation.GuardAreaId,
                        RhythmId = rhythmId,
                        UserId = GuardAreaAllocation.UserId
                    };

                    await _context.ShiftAllocations.AddAsync(shiftAllocation);
                    await _context.SaveChangesAsync();
                }
                
            }

            return RedirectToPage("./Index");
        }
    }
}
