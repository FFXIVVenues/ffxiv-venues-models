using System;
using TimeZoneConverter;

namespace FFXIVVenues.VenueModels.V2022
{
    public class Opening
    {
        public Day Day { get; set; }
        public Time Start { get; set; }
        public Time End { get; set; }
        public Location Location { get; set; }
        public bool IsNow => this.IsAt(DateTime.UtcNow);

        public bool IsAt(DateTime at)
        {
            var currentDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 0, 0, 0, DateTimeKind.Utc);
            var dayNumber = (int)currentDate.DayOfWeek - 1; // Monday is first day of the week for FFXIV Venues
            if (dayNumber == -1) dayNumber = 6;
            var weekBeginning = currentDate.AddDays(-dayNumber);

            var startTimeZone = TZConvert.GetTimeZoneInfo(this.Start.TimeZone);
            var start = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(
                    new DateTime(weekBeginning.Year, weekBeginning.Month, weekBeginning.Day, this.Start.Hour, this.Start.Minute, 0, DateTimeKind.Unspecified),
                    startTimeZone.Id, TimeZoneInfo.Utc.Id)
                .AddDays((int)this.Day + (this.Start.NextDay ? 1 : 0));

            var end = start.AddHours(3);
            if (this.End != null)
            {
                var endTimeZone = TZConvert.GetTimeZoneInfo(this.End.TimeZone);
                end = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(
                        new DateTime(weekBeginning.Year, weekBeginning.Month, weekBeginning.Day, this.End.Hour, this.End.Minute, 0, DateTimeKind.Unspecified),
                        endTimeZone.Id, TimeZoneInfo.Utc.Id)
                    .AddDays((int)this.Day + (this.End.NextDay ? 1 : 0));
            }

            return (at >= start && at < end);
        }
    }
}
