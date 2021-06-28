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

        public List<AttendanceTime> attendanceTimes { get; set; }

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

            shiftAllocationList = _context.ShiftAllocations.Include(a => a.User).ToList();

            rhythmDetails = _context.RhythmDetails.Include(a => a.Location).ToList();

            attendanceTimes = new List<AttendanceTime>
            {
                new AttendanceTime{ StartDate = "2021-06-01 08:00:00",EndDate = "2021-06-01 09:00:00", leave="true"},
                new AttendanceTime{ StartDate = "2021-06-01 09:00:00",EndDate = "2021-06-01 09:55:00", leave="false"},
                new AttendanceTime{ StartDate = "2021-06-01 09:55:00",EndDate = "2021-06-01 16:30:00", leave="true"}
            };

            int i = 0;

            foreach (DateTime day in Utils.EachDay(startDate, endDate))
            {
                foreach (var item in shiftAllocationList.Where(a => a.DateTime.Month == day.Month && a.DateTime.Day == day.Day))
                {
                    foreach (var item2 in rhythmDetails.Where(a => a.RhythmId == item.RhythmId))
                    {
                        //در این مرحله اتندنستایم هر نگهبان رو میگیریم و روی اون حلقه میزنیم
                        //در اینجا چون ای پی ای اماده نبود از دیتا این مموری استفاده کردیم
                        foreach (var item3 in attendanceTimes.Where(a => DateTime.Parse(a.StartDate).Month == day.Month && DateTime.Parse(a.StartDate).Day == day.Day))
                        {
                            if (item2.Time >= new TimeSpan(DateTime.Parse(item3.StartDate).Hour, DateTime.Parse(item3.StartDate).Minute, DateTime.Parse(item3.StartDate).Second) && item2.Time <= new TimeSpan(DateTime.Parse(item3.EndDate).Hour, DateTime.Parse(item3.EndDate).Minute, DateTime.Parse(item3.EndDate).Second) && item3.leave == "false")
                            {
                                //در اینجا جدول پلن رو پر میکنیم
                                //مقدار شیفت آی دی رو از طریق زمان، ریتم و محل گشت از جدول شیفت پیدا میکنیم
                                i++;
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
