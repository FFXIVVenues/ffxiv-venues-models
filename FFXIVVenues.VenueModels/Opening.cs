using System;
using TimeZoneConverter;

namespace FFXIVVenues.VenueModels
{
    public class Opening
    {
        public Day Day { get; set; }
        public Time Start { get; set; }
        public Time End { get; set; }
        public Location Location { get; set; }
        public bool IsNow => this.IsAt(DateTime.UtcNow);

        public bool IsAt(DateTime at) => this.Resolve(at).Open;

        public (bool Open, DateTime Start, DateTime End) Resolve(DateTime at)
        {
            var currentDate = new DateTime(at.Year, at.Month, at.Day, 0, 0, 0, at.Kind);
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
            if (end < start)
                end = end.AddDays(7);

            if (at >= start && at < end)
                return (true, start, end);

            if (this.Day == Day.Sunday && at < start)
            {
                var adjustedStart = start.AddDays(-7);
                var adjustedEnd = end.AddDays(-7);

                if (at >= adjustedStart && at < adjustedEnd)
                    return (true, adjustedStart, adjustedEnd);
            }

            return (at >= start && at < end, start, end);
        }
    }
}
