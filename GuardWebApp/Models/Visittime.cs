using System;
using System.Collections.Generic;

#nullable disable

namespace GuardWebApp.Models
{
    public partial class Visittime
    {
        public Visittime()
        {
            CheckLocationVisittimes = new HashSet<CheckLocationVisittime>();
        }

        public long Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan? StartTime { get; set; } = new TimeSpan(0, 0, 0);
        public TimeSpan? EndTime { get; set; } = new TimeSpan(23, 59, 59);
        public string Description { get; set; }
        public string Title { get; set; }

        public virtual ICollection<CheckLocationVisittime> CheckLocationVisittimes { get; set; }
    }
}
