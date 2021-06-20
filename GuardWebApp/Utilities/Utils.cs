using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace GuardWebApp.Utilities
{
    public static class Utils
    {
        public static string removeYear(this DateTime datetime)
        {
            return string.Format("{0}/{1}", datetime.Month, datetime.Day);
        }
    }
}
