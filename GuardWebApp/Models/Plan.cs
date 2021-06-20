using System;
using System.Collections.Generic;

#nullable disable

namespace GuardWebApp.Models
{
    public partial class Plan
    {
        public Plan()
        {
            Runs = new HashSet<Run>();
        }

        public long Id { get; set; }
        public long UserId { get; set; }
        public long ShiftId { get; set; }
        public DateTime DateTime { get; set; }
        public long LocationId { get; set; }

        public virtual Location Location { get; set; }
        public virtual Shift Shift { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Run> Runs { get; set; }
    }
}
