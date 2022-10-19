using System;

namespace FFXIVVenues.VenueModels
{
    public class UtcTime
    {
        public ushort Hour { get; init; }
        public ushort Minute { get; set; }
        public bool NextDay { get; set; }
        public string TimeZone => TimeZoneInfo.Utc.Id;
    }
}