using System;
using System.Collections.Generic;

#nullable disable

namespace GuardWebApp.Models
{
    public partial class ViolationType
    {
        public ViolationType()
        {
            ViolationConsequences = new HashSet<ViolationConsequence>();
            Violations = new HashSet<Violation>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public long ViolationNatureId { get; set; }

        public virtual ViolationNature ViolationNature { get; set; }
        public virtual ICollection<ViolationConsequence> ViolationConsequences { get; set; }
        public virtual ICollection<Violation> Violations { get; set; }
    }
}
