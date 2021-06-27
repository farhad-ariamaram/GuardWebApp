using System;
using System.Collections.Generic;

#nullable disable

namespace GuardWebApp.Models
{
    public partial class ViolationConsequence
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public long ViolationTypeId { get; set; }

        public virtual ViolationType ViolationType { get; set; }
    }
}
