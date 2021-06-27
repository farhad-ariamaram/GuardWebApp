using System;
using System.Collections.Generic;

#nullable disable

namespace GuardWebApp.Models
{
    public partial class CheckLocationVisittime
    {
        public long Id { get; set; }
        public long CheckLocationId { get; set; }
        public long VisittimeId { get; set; }

        public virtual CheckLocation CheckLocation { get; set; }
        public virtual Visittime Visittime { get; set; }
    }
}
