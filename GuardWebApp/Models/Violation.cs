using System;
using System.Collections.Generic;

#nullable disable

namespace GuardWebApp.Models
{
    public partial class Violation
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long ViolationTypeId { get; set; }
        public string Description { get; set; }
        public long RegistrarId { get; set; }
        public long ApproverId { get; set; }
        public long StatusId { get; set; }
        public DateTime ApproveDate { get; set; }
        public DateTime RegisterDate { get; set; }
        public long RunId { get; set; }

        public virtual User Approver { get; set; }
        public virtual User Registrar { get; set; }
        public virtual Run Run { get; set; }
        public virtual Status Status { get; set; }
        public virtual User User { get; set; }
        public virtual ViolationType ViolationType { get; set; }
    }
}
