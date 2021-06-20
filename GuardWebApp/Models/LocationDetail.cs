using System;
using System.Collections.Generic;

#nullable disable

namespace GuardWebApp.Models
{
    public partial class LocationDetail
    {
        public LocationDetail()
        {
            RunDetails = new HashSet<RunDetail>();
        }

        public long Id { get; set; }
        public long LocationId { get; set; }
        public long CheckLocationId { get; set; }
        public long? ClimateId { get; set; }

        public virtual CheckLocation CheckLocation { get; set; }
        public virtual Climate Climate { get; set; }
        public virtual Location Location { get; set; }
        public virtual ICollection<RunDetail> RunDetails { get; set; }
    }
}
