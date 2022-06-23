using System;

namespace FFXIVVenues.VenueModels.V2022
{
    public class OpenOverride
    {
        public bool Open { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public bool IsNow()
        {
            return DateTime.UtcNow > Start && DateTime.UtcNow < End;
        }
    }

}
