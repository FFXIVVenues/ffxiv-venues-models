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
        public string TimeZone { get => _timeZone; set => _timeZone = value; }
        public bool NextDay { get; set; }
        public UtcTime Utc => this.AsUtc();
        
        private UtcTime AsUtc()
        {
            var currentDay = DateTime.Now;
            var nextDay = currentDay.AddDays(1);
            var startTimeZone = TZConvert.GetTimeZoneInfo(this.TimeZone);
            var utcTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(
                    new DateTime(currentDay.Year, currentDay.Month, this.NextDay ? nextDay.Day : currentDay.Day, this.Hour, this.Minute, 0, DateTimeKind.Unspecified),
                    startTimeZone.Id, TimeZoneInfo.Utc.Id);
            return new UtcTime()
            {
                Hour = (ushort) utcTime.Hour,
                Minute = (ushort) utcTime.Minute,
                NextDay = utcTime.Day == nextDay.Day
            };
        }
    }

}
