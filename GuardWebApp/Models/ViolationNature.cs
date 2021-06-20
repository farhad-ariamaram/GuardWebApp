using System;
using System.Collections.Generic;

#nullable disable

namespace GuardWebApp.Models
{
    public partial class ViolationNature
    {
        public ViolationNature()
        {
            ViolationTypes = new HashSet<ViolationType>();
        }

        public long Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ViolationType> ViolationTypes { get; set; }
    }
}
