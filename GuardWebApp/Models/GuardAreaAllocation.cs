using System;
using System.Collections.Generic;

#nullable disable

namespace GuardWebApp.Models
{
    public partial class GuardAreaAllocation
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long GuardAreaId { get; set; }

        public virtual GuardArea GuardArea { get; set; }
        public virtual User User { get; set; }
    }
}
