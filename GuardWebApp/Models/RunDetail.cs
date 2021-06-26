
#nullable disable

namespace GuardWebApp.Models
{
    public partial class RunDetail
    {
        public long Id { get; set; }
        public long RunId { get; set; }
        public long? RunStatusId { get; set; }
        public long LocationDetailId { get; set; }

        public virtual LocationDetail LocationDetail { get; set; }
        public virtual Run Run { get; set; }
        public virtual RunStatus RunStatus { get; set; }
    }
}
