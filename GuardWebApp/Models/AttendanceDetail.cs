using System;
using System.Collections.Generic;

#nullable disable

namespace GuardWebApp.Models
{
    public partial class AttendanceDetail
    {
        public long Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Leave { get; set; }
        public long AttendanceId { get; set; }

        public virtual Attendance Attendance { get; set; }
    }
}
