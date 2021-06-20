using System;
using System.Collections.Generic;

#nullable disable

namespace GuardWebApp.Models
{
    public partial class GuardArea
    {
        public GuardArea()
        {
            Locations = new HashSet<Location>();
            Rhythms = new HashSet<Rhythm>();
            ShiftAllocations = new HashSet<ShiftAllocation>();
            Shifts = new HashSet<Shift>();
        }

        public long Id { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Location> Locations { get; set; }
        public virtual ICollection<Rhythm> Rhythms { get; set; }
        public virtual ICollection<ShiftAllocation> ShiftAllocations { get; set; }
        public virtual ICollection<Shift> Shifts { get; set; }
    }
}
