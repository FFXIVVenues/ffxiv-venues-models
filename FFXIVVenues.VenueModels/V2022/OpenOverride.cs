using System;

namespace FFXIVVenues.VenueModels.V2022
{
    public class OpenOverride
    {
        public bool Open { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool IsNow => this.IsNowInternal();

        private bool IsNowInternal()
        {
            return DateTime.UtcNow > Start.ToUniversalTime() && DateTime.UtcNow < End.ToUniversalTime();
        }
    }

}
