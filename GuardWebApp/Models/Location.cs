using System;
using System.Collections.Generic;

#nullable disable

namespace GuardWebApp.Models
{
    public partial class Location
    {
        public Location()
        {
            CheckLocations = new HashSet<CheckLocation>();
            LocationDetails = new HashSet<LocationDetail>();
            Plans = new HashSet<Plan>();
            RhythmDetails = new HashSet<RhythmDetail>();
            SubmittedLocations = new HashSet<SubmittedLocation>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Qr { get; set; }
        public string Nfc { get; set; }
        public long GuardAreaId { get; set; }

        public virtual GuardArea GuardArea { get; set; }
        public virtual ICollection<CheckLocation> CheckLocations { get; set; }
        public virtual ICollection<LocationDetail> LocationDetails { get; set; }
        public virtual ICollection<Plan> Plans { get; set; }
        public virtual ICollection<RhythmDetail> RhythmDetails { get; set; }
        public virtual ICollection<SubmittedLocation> SubmittedLocations { get; set; }
    }
}
