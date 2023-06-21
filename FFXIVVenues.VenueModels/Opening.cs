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

        // todo: Remove these two fields, let client handle it by JS / .NET (by FFXIV Venues SDK)
        // Warning: Everything is compatable with IANA IDs, but not all data in dbs are IANA. Will need
        // a fixer to move these to IANA ids. Maybe move TimeZone up to Opening (instead of Time) at the same time. 
        public Opening Utc => this.ToUtc();
        public bool IsNow => this.IsAt(DateTime.UtcNow);

        public bool IsAt(DateTime at) => 
            this.Resolve(at).Open;

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

        public Opening ToUtc()
        {
            // Create a new Opening object to hold the adjusted values
            var adjustedOpening = new Opening
            {
                Day = this.Day,
                Start = new Time
                {
                    Hour = this.Start.Hour,
                    Minute = this.Start.Minute,
                    TimeZone = "UTC",
                    NextDay = this.Start.NextDay
                },
                End = new Time
                {
                    Hour = this.End.Hour,
                    Minute = this.End.Minute,
                    TimeZone = "UTC",
                    NextDay = this.End.NextDay
                }
            };
        
            var timezoneOffsetMinutes = -TZConvert.GetTimeZoneInfo(this.Start.TimeZone)
                .GetUtcOffset(DateTime.UtcNow).TotalMinutes;
        
            // Adjust the start time
            var totalStartMinutes = adjustedOpening.Start.Hour * 60 + adjustedOpening.Start.Minute + timezoneOffsetMinutes;
            var rolledStartMinutes = totalStartMinutes;
            if (totalStartMinutes < 0 && adjustedOpening.Start.NextDay)
            {
                rolledStartMinutes += 24 * 60;
                adjustedOpening.Start.NextDay = false;
            }
            else if (totalStartMinutes < 0)
            {
                rolledStartMinutes += 24 * 60;
                adjustedOpening.Day = adjustedOpening.Day.Previous();
            }
            else if (totalStartMinutes >= 24 * 60 && adjustedOpening.Start.NextDay)
            {
                rolledStartMinutes -= 24 * 60;
                adjustedOpening.Start.NextDay = false;
                adjustedOpening.Day = adjustedOpening.Day.Next(2);
            }
            else if (totalStartMinutes >= 24 * 60)
            {
                rolledStartMinutes -= 24 * 60;
                adjustedOpening.Day = adjustedOpening.Day.Next();
            } 
            else if (adjustedOpening.Start.NextDay)
            {
                adjustedOpening.Start.NextDay = false;
                adjustedOpening.Day = adjustedOpening.Day.Next();
            }
        
            adjustedOpening.Start.Hour = (ushort)(rolledStartMinutes / 60);
            adjustedOpening.Start.Minute = (ushort)(rolledStartMinutes % 60);
        
            // Adjust the end time
            var totalEndMinutes = adjustedOpening.End.Hour * 60 + adjustedOpening.End.Minute + timezoneOffsetMinutes;
            var rolledEndMinutes = totalEndMinutes;
            
            if (totalEndMinutes < 0)
                rolledEndMinutes += 24 * 60;
            else if (totalEndMinutes >= 24 * 60)
                rolledEndMinutes -= 24 * 60;

            adjustedOpening.End.Hour = (ushort)(rolledEndMinutes / 60);
            adjustedOpening.End.Minute = (ushort)(rolledEndMinutes % 60);
            adjustedOpening.End.NextDay = rolledEndMinutes < rolledStartMinutes;
        
            // Return the adjusted Opening object
            return adjustedOpening;
        }

    }
}
