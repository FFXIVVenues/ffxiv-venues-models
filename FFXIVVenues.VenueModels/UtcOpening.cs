using System;

namespace FFXIVVenues.VenueModels
{
    public class UtcOpening
    {
        public Day Day { get; set; }
        public Time Start { get; set; }
        public Time End { get; set; }
        public Location Location { get; set; }
    }
}