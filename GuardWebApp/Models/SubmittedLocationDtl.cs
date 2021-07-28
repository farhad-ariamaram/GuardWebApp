using System;
using System.Collections.Generic;

#nullable disable

namespace GuardWebApp.Models
{
    public partial class SubmittedLocationDtl
    {
        public long Id { get; set; }
        public long SubmittedLocationId { get; set; }
        public long LocationDetailId { get; set; }
        public long RunStatusId { get; set; }

        public virtual LocationDetail LocationDetail { get; set; }
        public virtual RunStatus RunStatus { get; set; }
        public virtual SubmittedLocation SubmittedLocation { get; set; }
    }
}
