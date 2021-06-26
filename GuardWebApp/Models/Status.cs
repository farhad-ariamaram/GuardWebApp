
using System.Collections.Generic;

#nullable disable

namespace GuardWebApp.Models
{
    public partial class Status
    {
        public Status()
        {
            Runs = new HashSet<Run>();
            Violations = new HashSet<Violation>();
        }

        public long Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Run> Runs { get; set; }
        public virtual ICollection<Violation> Violations { get; set; }
    }
}
