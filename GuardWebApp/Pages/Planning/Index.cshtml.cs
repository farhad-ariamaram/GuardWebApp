using System;
using System.Collections.Generic;
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
                    startDate = new DateTime(2000, int.Parse(sDate), 1);
                    endDate = new DateTime(2000, int.Parse(sDate), DateTime.DaysInMonth(2000, int.Parse(sDate)));
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
                        }
                        else
                        {
                            Shift shift1 = new Shift
                            {
                                GuardAreaId = guardArea,
                                RhythmId = rhythm,
                                DateTime = new DateTime(2000, day.Month, day.Day)
                            };

                            _context.Shifts.Add(shift1);
                            _context.SaveChanges();

                            shift = shift1.Id;
                        }

                        //در این مرحله حضور نگهبان در این روز را بوسیله آی دی نگهبان که در بالا بدست اوردیم و روز(دٍی) میگیریم
                        //در اینجا چون ای پی ای اماده نبود از دیتا این مموری استفاده کردیم
                        List<AttendanceTime> attendanceTimes = new List<AttendanceTime>
                        {
                            new AttendanceTime{ StartDate = "2021-06-01 08:00:00",EndDate = "2021-06-01 09:00:00", leave="false"},
                            new AttendanceTime{ StartDate = "2021-06-01 09:00:00",EndDate = "2021-06-01 09:55:00", leave="false"},
                            new AttendanceTime{ StartDate = "2021-06-01 09:55:00",EndDate = "2021-06-01 16:30:00", leave="false"}
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
                                    DateTime = new DateTime(2000, day.Month, day.Day, time.Hours, time.Minutes, time.Seconds)
                                };
                                if (!_context.Plans.Where(a => a.UserId == guard && a.LocationId == location && a.ShiftId == shift && a.DateTime == new DateTime(2000, day.Month, day.Day, time.Hours, time.Minutes, time.Seconds)).Any())
                                {
                                    _context.Plans.Add(plan);
                                    _context.SaveChanges();
                                }

                            }
                        }
                    }
                }
            }

            ViewData["step2"] = true;
            return Page();
        }

    }
}