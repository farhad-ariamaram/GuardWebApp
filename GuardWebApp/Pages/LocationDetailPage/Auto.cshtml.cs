using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuardWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GuardWebApp.Pages.LocationDetailPage
{
    public class AutoModel : PageModel
    {
        private readonly GuardianDBContext _db;

        public AutoModel(GuardianDBContext db)
        {
            _db = db;
        }

        public async Task<ActionResult> OnGet(long? LocationId)
        {
            if (!LocationId.HasValue)
            {
                return RedirectToPage("../Panel");
            }

            var loc = await _db.Locations.FindAsync(LocationId);
            if (loc == null)
            {
                return RedirectToPage("../Panel");
            }

            var CheckLocationList = await _db.CheckLocations.Where(a => a.LocationId == LocationId).ToListAsync();

            if (CheckLocationList.Any())
            {
                foreach (var item in CheckLocationList)
                {
                    var chklocvistimList = _db.CheckLocationVisittimes.Include(b => b.Visittime).Where(a => a.CheckLocationId == item.Id);
                    if (await chklocvistimList.AnyAsync())
                    {
                        foreach (var itom in chklocvistimList)
                        {
                            LocationDetail ld = new LocationDetail
                            {
                                CheckId = item.CheckId,
                                ClimateId = item.ClimateId==null ? null : item.ClimateId,
                                LocationId = LocationId.Value,
                                StartDate = itom.Visittime.StartDate,
                                EndDate = itom.Visittime.EndDate,
                                StartTime = itom.Visittime.StartTime,
                                EndTime = itom.Visittime.EndTime
                            };

                            await _db.AddAsync(ld);
                        }
                    }
                    else
                    {
                        LocationDetail ld = new LocationDetail
                        {
                            CheckId = item.CheckId,
                            ClimateId = item.ClimateId == null ? null : item.ClimateId,
                            LocationId = LocationId.Value,
                            StartDate = null,
                            EndDate = null,
                            StartTime = null,
                            EndTime = null
                        };

                        await _db.AddAsync(ld);
                    }

                    await _db.SaveChangesAsync();
                }
            }





            return Page();
        }
    }
}
