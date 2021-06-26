using System;

#nullable disable

namespace GuardWebApp.Models
{
    public partial class RhythmDetail
    {
        public long Id { get; set; }
        public long RhythmId { get; set; }
        public long LocationId { get; set; }
        public long OrderNo { get; set; }
        public TimeSpan Time { get; set; }

        public virtual Location Location { get; set; }
        public virtual Rhythm Rhythm { get; set; }
    }
}
