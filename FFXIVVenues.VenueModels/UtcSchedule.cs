using System;

namespace FFXIVVenues.VenueModels
{
    public class UtcSchedule
    {
        public DateTimeOffset? From { get; set; }
        public Day Day { get; set; }
        public Time Start { get; set; }
        public Time End { get; set; }
        public Location Location { get; set; }
    }
}