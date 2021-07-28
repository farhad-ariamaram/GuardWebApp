using System;
using System.Collections.Generic;

#nullable disable

namespace GuardWebApp.Models
{
    public partial class SubmittedLocation
    {
        public SubmittedLocation()
        {
            Runs = new HashSet<Run>();
            SubmittedLocationDtls = new HashSet<SubmittedLocationDtl>();
        }

        public long Id { get; set; }
        public long LocationId { get; set; }
        public long UserId { get; set; }
        public DateTime DateTime { get; set; }
        public long DeviceId { get; set; }

        public virtual Device Device { get; set; }
        public virtual Location Location { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Run> Runs { get; set; }
        public virtual ICollection<SubmittedLocationDtl> SubmittedLocationDtls { get; set; }
    }
}
