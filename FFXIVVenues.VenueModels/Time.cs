using System;
using TimeZoneConverter;

namespace FFXIVVenues.VenueModels
{
    public class Time
    {
        private ushort _hour;
        private ushort _minute;
        private string _timeZone = TimeZoneInfo.Utc.Id;

        public ushort Hour { get => _hour; set { if (value < 0 || value > 23) throw new ArgumentOutOfRangeException(nameof(Hour), $"Hour must be set to an integer between 0 and 23 (inclusive)."); _hour = value; } }
        public ushort Minute { get => _minute; set { if (value < 0 || value > 59) throw new ArgumentOutOfRangeException(nameof(Minute), $"Minute must be set to an integer between 0 and 59 (inclusive)."); _minute = value; } }
        // todo: move this to Opening
        public string TimeZone { get; set; } = TimeZoneInfo.Utc.Id;
        public bool NextDay { get; set; }
    }

}
