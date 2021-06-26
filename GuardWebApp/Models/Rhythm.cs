
using System.Collections.Generic;

#nullable disable

namespace GuardWebApp.Models
{
    public partial class Rhythm
    {
        public Rhythm()
        {
            RhythmDetails = new HashSet<RhythmDetail>();
            ShiftAllocations = new HashSet<ShiftAllocation>();
            Shifts = new HashSet<Shift>();
        }

        public long Id { get; set; }
        public string Title { get; set; }
        public long GuardAreaId { get; set; }

        public virtual GuardArea GuardArea { get; set; }
        public virtual ICollection<RhythmDetail> RhythmDetails { get; set; }
        public virtual ICollection<ShiftAllocation> ShiftAllocations { get; set; }
        public virtual ICollection<Shift> Shifts { get; set; }
    }
}
