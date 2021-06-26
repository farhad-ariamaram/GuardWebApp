
using System.Collections.Generic;

#nullable disable

namespace GuardWebApp.Models
{
    public partial class User
    {
        public User()
        {
            Plans = new HashSet<Plan>();
            RunApprovers = new HashSet<Run>();
            RunUsers = new HashSet<Run>();
            SubmittedLocations = new HashSet<SubmittedLocation>();
            ViolationApprovers = new HashSet<Violation>();
            ViolationRegistrars = new HashSet<Violation>();
            ViolationUsers = new HashSet<Violation>();
        }

        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public virtual ICollection<Plan> Plans { get; set; }
        public virtual ICollection<Run> RunApprovers { get; set; }
        public virtual ICollection<Run> RunUsers { get; set; }
        public virtual ICollection<SubmittedLocation> SubmittedLocations { get; set; }
        public virtual ICollection<Violation> ViolationApprovers { get; set; }
        public virtual ICollection<Violation> ViolationRegistrars { get; set; }
        public virtual ICollection<Violation> ViolationUsers { get; set; }
    }
}
