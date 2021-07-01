using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

        public IActionResult OnPostDate(string sDate, string timetype)
        {
            PersianCalendar pc = new PersianCalendar();

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
                        if (_context.Shifts.Where(a => a.DateTime.Month == day.Month && a.DateTime.Day == day.Day && a.GuardAreaId == guardArea && a.RhythmId == rhythm).Any())
                        {
                            shift = _context.Shifts.FirstOrDefault(a => a.DateTime.Month == day.Month && a.DateTime.Day == day.Day && a.GuardAreaId == guardArea && a.RhythmId == rhythm).Id;

                            //در این مرحله حضور نگهبان در این روز را بوسیله آی دی نگهبان که در بالا بدست اوردیم و روز(دٍی) میگیریم
                            //در اینجا چون ای پی ای اماده نبود از دیتا این مموری استفاده کردیم
                            List<AttendanceTime> attendanceTimes = new List<AttendanceTime>
                            {
                                new AttendanceTime{ StartDate = "2021-03-21 08:00:00",EndDate = "2021-03-21 16:30:00", leave="false"},
                                new AttendanceTime{ StartDate = "2021-03-22 09:00:00",EndDate = "2021-03-22 16:30:00", leave="false"},
                                new AttendanceTime{ StartDate = "2021-03-23 09:55:00",EndDate = "2021-03-23 16:30:00", leave="false"}
                            };

                            foreach (var attendanceTime in attendanceTimes)
                            {
                                if (time >= new TimeSpan(DateTime.Parse(attendanceTime.StartDate).Hour, DateTime.Parse(attendanceTime.StartDate).Minute, DateTime.Parse(attendanceTime.StartDate).Second) &&
                                    time < new TimeSpan(DateTime.Parse(attendanceTime.EndDate).Hour, DateTime.Parse(attendanceTime.EndDate).Minute, DateTime.Parse(attendanceTime.EndDate).Second) &&
                                    attendanceTime.leave == "false")
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