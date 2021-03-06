using System;
using System.Collections.Generic;

#nullable disable

namespace GuardWebApp.Models
{
    public partial class ShiftAllocation
    {
        public long Id { get; set; }
        public long GuardAreaId { get; set; }
        public long RhythmId { get; set; }
        public DateTime DateTime { get; set; }
        public long UserId { get; set; }

        public virtual GuardArea GuardArea { get; set; }
        public virtual Rhythm Rhythm { get; set; }
        public virtual User User { get; set; }
    }
}
