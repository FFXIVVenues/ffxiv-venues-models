using System;

namespace FFXIVVenues.VenueModels
{
    public class UtcSchedule
    {
        public Day Day { get; set; }
        public Time Start { get; set; }
        public Time End { get; set; }
        public Location Location { get; set; }
    }
}