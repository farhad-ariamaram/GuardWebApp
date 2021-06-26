
using System.Collections.Generic;

#nullable disable

namespace GuardWebApp.Models
{
    public partial class Device
    {
        public Device()
        {
            SubmittedLocations = new HashSet<SubmittedLocation>();
        }

        public long Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<SubmittedLocation> SubmittedLocations { get; set; }
    }
}
