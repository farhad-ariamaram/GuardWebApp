
using System.Collections.Generic;

#nullable disable

namespace GuardWebApp.Models
{
    public partial class Run
    {
        public Run()
        {
            RunDetails = new HashSet<RunDetail>();
            Violations = new HashSet<Violation>();
        }

        public long Id { get; set; }
        public long PlanId { get; set; }
        public long UserId { get; set; }
        public long ApproverId { get; set; }
        public long? SubmittedLocationId { get; set; }
        public long StatusId { get; set; }

        public virtual User Approver { get; set; }
        public virtual Plan Plan { get; set; }
        public virtual Status Status { get; set; }
        public virtual SubmittedLocation SubmittedLocation { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<RunDetail> RunDetails { get; set; }
        public virtual ICollection<Violation> Violations { get; set; }
    }
}
