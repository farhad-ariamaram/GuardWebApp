using System;
using System.Collections.Generic;

namespace GuardWebApp.Utilities
{
    public static class Utils
    {
        public static string removeYear(this DateTime datetime)
        {
            return string.Format("{0}/{1}", datetime.Month, datetime.Day);
        }

        public static IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }
    }
}
