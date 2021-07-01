using System;
using System.Collections.Generic;
using System.Globalization;

namespace GuardWebApp.Utilities
{
    public static class Utils
    {
        public static string removeYear(this DateTime datetime)
        {
            PersianCalendar pc = new PersianCalendar();
            return string.Format("{0}/{1}", pc.GetMonth(datetime), pc.GetDayOfMonth(datetime));
        }

        public static string getPersianDay(this DateTime datetime)
        {
            PersianCalendar pc = new PersianCalendar();
            return string.Format("{0}", pc.GetDayOfMonth(datetime));
        }


        public static IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }
    }
}
