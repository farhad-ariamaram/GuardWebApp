using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using GuardWebApp.Controllers;
using GuardWebApp.Models;
using GuardWebApp.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static GuardWebApp.Controllers.UsersController;

namespace GuardWebApp.Pages.Planning
{
    public class IndexModel : PageModel
    {
        private readonly GuardianDBContext _context;

        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }

        public List<ShiftAllocation> shiftAllocationList { get; set; }
        public List<RhythmDetail> rhythmDetails { get; set; }


        public IndexModel(GuardianDBContext context)
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

            ViewData["step1"] = true;
            return Page();
        }

        public IActionResult OnGetStep2()
        {
            ViewData["step1"] = null;
            ViewData["step2"] = true;
            return Page();
        }

        public async Task<ActionResult> OnPostDate(string sDate, string timetype)
        {
            var uid = HttpContext.Session.GetString("uid");
            if (uid == null)
            {
                return RedirectToPage("../Index");
            }

            PersianCalendar pc = new PersianCalendar();
            UsersController api = new UsersController(_context);

            switch (timetype)
            {
                case "tofrom":
                    startDate = new System.DateTime(1400, int.Parse(Request.Form["smonthField"].ToString()), int.Parse(Request.Form["sdayField"].ToString()), pc);
                    endDate = new System.DateTime(1400, int.Parse(Request.Form["emonthField"].ToString()), int.Parse(Request.Form["edayField"].ToString()), pc);
                    break;
                case "daily":
                    startDate = new System.DateTime(1400, int.Parse(Request.Form["monthField"].ToString()), int.Parse(Request.Form["dayField"].ToString()), pc);
                    endDate = startDate;
                    break;
                case "monthly":
                    startDate = new DateTime(1400, int.Parse(sDate), 1, pc);
                    int lastdaymonth = 29;
                    int[] first6month = { 1, 2, 3, 4, 5, 6 };
                    int[] second6month = { 7, 8, 9, 10, 11 };
                    if (first6month.Contains(int.Parse(sDate)))
                    {
                        lastdaymonth = 31;
                    }
                    else if (second6month.Contains(int.Parse(sDate)))
                    {
                        lastdaymonth = 30;
                    }
                    endDate = new DateTime(1400, int.Parse(sDate), lastdaymonth, pc);
                    break;
                default:
                    break;
            }

            shiftAllocationList = _context.ShiftAllocations.ToList();

            rhythmDetails = _context.RhythmDetails.ToList();

            foreach (DateTime day in Utils.EachDay(startDate, endDate))
            {
                foreach (var shiftAllocation in shiftAllocationList.Where(a => a.DateTime.Month == day.Month && a.DateTime.Day == day.Day))
                {
                    foreach (var rhythmDetail in rhythmDetails.Where(a => a.RhythmId == shiftAllocation.RhythmId))
                    {

                        // روز = day
                        var time = rhythmDetail.Time;
                        var orderNo = rhythmDetail.OrderNo;
                        var location = rhythmDetail.LocationId;
                        var guard = shiftAllocation.UserId;
                        var guardArea = shiftAllocation.GuardAreaId;
                        var rhythm = shiftAllocation.RhythmId;
                        long shift = 0;
                        var attendanceListAsync = await api.Attend(uid, $"{guard}", day.ToShortDateString());

                        List<AttendanceTime> attendanceList = null;
                        if (attendanceListAsync != null)
                        {
                            attendanceList = attendanceListAsync.Value;
                            var at = _context.Attendances.Where(a => a.DateTime == day);
                            if (!at.Any())
                            {
                                Attendance attendance = new Attendance
                                {
                                    DateTime = day,
                                    GuardId = guard
                                };
                                await _context.Attendances.AddAsync(attendance);
                                await _context.SaveChangesAsync();

                                foreach (var item in attendanceList)
                                {
                                    await _context.AttendanceDetails.AddAsync(new AttendanceDetail
                                    {
                                        AttendanceId = attendance.Id,
                                        StartDate = DateTime.Parse(item.StartDate),
                                        EndDate = DateTime.Parse(item.EndDate),
                                        Leave = (item.leave == "flase" ? false : true)
                                    });
                                }
                                await _context.SaveChangesAsync();
                            }
                            
                        }
                        else
                        {
                            continue;
                        }



                        if (_context.Shifts.Where(a => a.DateTime.Month == day.Month && a.DateTime.Day == day.Day && a.GuardAreaId == guardArea && a.RhythmId == rhythm).Any())
                        {
                            shift = _context.Shifts.FirstOrDefault(a => a.DateTime.Month == day.Month && a.DateTime.Day == day.Day && a.GuardAreaId == guardArea && a.RhythmId == rhythm).Id;
                            bool flag = false;

                            if (attendanceList != null)
                            {
                                foreach (var attendanceTime in attendanceList)
                                {
                                    if (day.Month == DateTime.Parse(attendanceTime.StartDate).Month && day.Day == DateTime.Parse(attendanceTime.EndDate).Day)
                                    {
                                        var start = new TimeSpan(DateTime.Parse(attendanceTime.StartDate).Hour, DateTime.Parse(attendanceTime.StartDate).Minute,0);
                                        var end = new TimeSpan(DateTime.Parse(attendanceTime.EndDate).Hour, DateTime.Parse(attendanceTime.EndDate).Minute,0);

                                        if (time >= start &&
                                        time <= end &&
                                        attendanceTime.leave == "flase")
                                        {
                                            flag = true;
                                        }

                                        if (time >= new TimeSpan(DateTime.Parse(attendanceTime.StartDate).Hour, DateTime.Parse(attendanceTime.StartDate).Minute, DateTime.Parse(attendanceTime.StartDate).Second) &&
                                        time <= new TimeSpan(DateTime.Parse(attendanceTime.EndDate).Hour, DateTime.Parse(attendanceTime.EndDate).Minute, DateTime.Parse(attendanceTime.EndDate).Second) &&
                                        attendanceTime.leave == "true")
                                        {
                                            flag = false;
                                            break;
                                        }
                                    }
                                }
                            }

                            if (flag)
                            {
                                Plan plan = new Plan
                                {
                                    UserId = guard,
                                    LocationId = location,
                                    ShiftId = shift,
                                    DateTime = new DateTime(2021, day.Month, day.Day, time.Hours, time.Minutes, time.Seconds)
                                };
                                if (!_context.Plans.Where(a => a.UserId == guard && a.LocationId == location && a.ShiftId == shift && a.DateTime == new DateTime(1400, day.Month, day.Day, time.Hours, time.Minutes, time.Seconds, pc)).Any())
                                {
                                    _context.Plans.Add(plan);
                                    _context.SaveChanges();
                                }
                            }
                        }
                        //else
                        //{
                        //    Shift shift1 = new Shift
                        //    {
                        //        GuardAreaId = guardArea,
                        //        RhythmId = rhythm,
                        //        DateTime = new DateTime(2000, day.Month, day.Day)
                        //    };

                        //    _context.Shifts.Add(shift1);
                        //    _context.SaveChanges();

                        //    shift = shift1.Id;
                        //}
                    }
                }
            }

            ViewData["step1"] = null;
            ViewData["step2"] = null;
            ViewData["step3"] = true;
            return Page();
        }

    }
}