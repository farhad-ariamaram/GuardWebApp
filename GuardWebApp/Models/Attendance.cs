using System;
using System.Collections.Generic;

#nullable disable

namespace GuardWebApp.Models
{
    public partial class Attendance
    {
        public Attendance()
        {
            AttendanceDetails = new HashSet<AttendanceDetail>();
        }

        public long Id { get; set; }
        public long GuardId { get; set; }
        public DateTime DateTime { get; set; }

        public virtual ICollection<AttendanceDetail> AttendanceDetails { get; set; }
    }
}
