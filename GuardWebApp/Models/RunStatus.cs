
using System.Collections.Generic;

#nullable disable

namespace GuardWebApp.Models
{
    public partial class RunStatus
    {
        public RunStatus()
        {
            RunDetails = new HashSet<RunDetail>();
        }

        public long Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<RunDetail> RunDetails { get; set; }
    }
}
