using System;

namespace FFXIVVenues.VenueModels
{
    public class ScheduleOverride
    {
        public bool Open { get; set; }
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset End { get; set; }
        public bool IsNow => this.IsNowInternal();

        private bool IsNowInternal()
        {
            return DateTimeOffset.UtcNow > Start.ToUniversalTime() && DateTimeOffset.UtcNow < End.ToUniversalTime();
        }
    }

}
