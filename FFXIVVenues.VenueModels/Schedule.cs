﻿using System;
using TimeZoneConverter;

namespace FFXIVVenues.VenueModels
{
    public class Schedule
    {
        
        public Day Day { get; set; }
        public Time Start { get; set; }
        public Time End { get; set; }
        public Interval Interval { get; set; }
        public Location Location { get; set; }

        // todo: Remove these two fields (leave just methods), let client handle it by JS / .NET (by FFXIV Venues SDK)
        // Warning: Everything is compatable with IANA IDs, but not all data in dbs are IANA. Will need
        // a fixer to move these to IANA ids. Maybe move TimeZone up to Opening (instead of Time) at the same time. 
        // This will mean Utc field is not needed even on client, because it can recognise the IANA ID and convert 
        // direct from Venue's source timezone to local.
        public UtcSchedule Utc => this.ToUtc();
        public bool WithinWeek => this.WithinWeekOf(DateTime.UtcNow);
        public bool IsNow => this.IsAt(DateTimeOffset.UtcNow);

        public bool IsAt(DateTimeOffset at) => 
            this.Resolve(at).Open;
        
        public (bool Open, DateTimeOffset Start, DateTimeOffset End) Resolve(DateTimeOffset at)
        {
            var enumerator = new ScheduleEnumerator(this, at);
            while (enumerator.MoveNext() && enumerator.Current?.End < at) continue;
            
            var isNow = at >= enumerator.Current!.Start && at < enumerator.Current!.End;
            return (isNow, enumerator.Current!.Start, enumerator.Current!.End);
        }

        public (bool Open, DateTimeOffset Start, DateTimeOffset End) OldResolve(DateTimeOffset at)
        {
            var currentDate = new DateTimeOffset(at.Year, at.Month, at.Day, 0, 0, 0, TimeSpan.Zero);
            var dayNumber = (int)currentDate.DayOfWeek - 1; // Monday is first day of the week for FFXIV Venues
            if (dayNumber == -1) dayNumber = 6;
            var weekBeginning = currentDate.AddDays(-dayNumber);
            
            var startTimeZone = TZConvert.GetTimeZoneInfo(this.Start.TimeZone);
            var timeZoneOffset = startTimeZone.GetUtcOffset(at);
            var start = new DateTimeOffset(weekBeginning.Year, weekBeginning.Month, weekBeginning.Day, this.Start.Hour, this.Start.Minute, 0, timeZoneOffset)
                .AddDays((int)this.Day + (this.Start.NextDay ? 1 : 0));

            var end = start.AddHours(3);
            if (this.End != null)
            {
                end = new DateTimeOffset(weekBeginning.Year, weekBeginning.Month, weekBeginning.Day, this.End.Hour, this.End.Minute, 0, timeZoneOffset)
                    .AddDays((int)this.Day + (this.End.NextDay ? 1 : 0));
            }
            if (end < start)
                end = end.AddDays(7);

            if (!this.WithinWeekOf(at))
                return (false, start, end);

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

        public bool WithinWeekOf(DateTimeOffset currentDate)
        {
            if ((this.Interval?.IntervalArgument ?? 1) == 1)
                return true;
            
            var daysBetween = (int) (currentDate - this.Interval.IntervalFrom).TotalDays;
            if (daysBetween == 0)
                return true;

            if (daysBetween < 0)
                return daysBetween > -7;

            var weekNumber = (daysBetween / 7) + 1;
            return weekNumber % this.Interval.IntervalArgument == 0;
        }
        
        public UtcSchedule ToUtc()
        {
            // Create a new Opening object to hold the adjusted values
            var adjustedOpening = new UtcSchedule
            {
                Day = this.Day,
                Location = this.Location,
                Start = new Time
                {
                    Hour = this.Start.Hour,
                    Minute = this.Start.Minute,
                    TimeZone = "UTC",
                    NextDay = this.Start.NextDay
                },
                End = this.End == null ? null : new Time
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

            if (adjustedOpening.End == null)
                return adjustedOpening;
            
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
