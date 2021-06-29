﻿using System;
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

            if (GuardAreaAllocation.EndDate == null)
            {
                GuardAreaAllocation.EndDate = new DateTime(GuardAreaAllocation.StartDate.Year, 12, 29);
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
                    if (_context.Shifts.FirstOrDefault(a => a.GuardAreaId == GuardAreaAllocation.GuardAreaId && a.DateTime.Month == day.Month && a.DateTime.Day == day.Day) != null)
                    {
                        long rhythmId = _context.Shifts.FirstOrDefault(a => a.GuardAreaId == GuardAreaAllocation.GuardAreaId && a.DateTime.Month == day.Month && a.DateTime.Day == day.Day).RhythmId;
                        ShiftAllocation shifttAllocation = new ShiftAllocation
                        {
                            DateTime = day,
                            GuardAreaId = GuardAreaAllocation.GuardAreaId,
                            RhythmId = rhythmId,
                            UserId = GuardAreaAllocation.UserId
                        };

                        await _context.ShiftAllocations.AddAsync(shifttAllocation);
                        await _context.SaveChangesAsync();
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

            return RedirectToPage("./Index");
        }

        private bool GuardAreaAllocationExists(long id)
        {
            return _context.GuardAreaAllocations.Any(e => e.Id == id);
        }
    }
}