using System;
using System.Collections.Generic;

#nullable disable

namespace GuardWebApp.Models
{
    public partial class CheckLocation
    {
        public CheckLocation()
        {
            CheckLocationVisittimes = new HashSet<CheckLocationVisittime>();
            LocationDetails = new HashSet<LocationDetail>();
        }

        public long Id { get; set; }
        public long LocationId { get; set; }
        public long CheckId { get; set; }
        public long? ClimateId { get; set; }
        public string Description { get; set; }

        public virtual Check Check { get; set; }
        public virtual Climate Climate { get; set; }
        public virtual Location Location { get; set; }
        public virtual ICollection<CheckLocationVisittime> CheckLocationVisittimes { get; set; }
        public virtual ICollection<LocationDetail> LocationDetails { get; set; }
    }
}
