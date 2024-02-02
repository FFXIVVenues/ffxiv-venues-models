using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TimeZoneConverter;

namespace FFXIVVenues.VenueModels;

public class ScheduleEnumerator : IEnumerator<Opening>
{

    private readonly Schedule _schedule;
    private readonly DateTimeOffset _from;
    private readonly int _interval;
    private readonly TimeZoneInfo _timeZone;

    public ScheduleEnumerator(Schedule schedule, DateTimeOffset? fromFallback = null)
    {
        this._schedule = schedule;
        this._from = this._schedule.Commencing ?? fromFallback 
                   ?? throw new ArgumentNullException(nameof(fromFallback), "No start point for enumeration given; provide either a fallback argument or a schedule with a IntervalFrom.");
        this._interval = this._schedule.Interval?.IntervalArgument ?? 1;
        this._timeZone = TZConvert.GetTimeZoneInfo(this._schedule.Start.TimeZone);
    }

    public bool MoveNext()
    {
        if (this._schedule.Interval.IntervalType == IntervalType.EveryXWeeks)
            return this.MoveNextByXWeeks();
        if (this._schedule.Interval.IntervalType == IntervalType.EveryXthDayOfTheMonth)
            return this.MoveNextByXthDayOfMonth();
        return false;
    }
    
    private bool MoveNextByXWeeks()
    {
        if (this.Current != null)
        {
            this.Current = this.Current
                .AddDays(7 * this._interval)
                .SetOffset(_timeZone);
            return true;
        }
        
        var end = this._schedule.End ?? new Time
        {
            Hour = (ushort)((this._schedule.Start.Hour + 3) % 24),
            Minute = this._schedule.Start.Minute,
            TimeZone = this._schedule.Start.TimeZone
        };

        var basis = this._from;
        var openingOffset = this._timeZone.GetUtcOffset(basis);
        if (basis.Offset != openingOffset)
            basis = basis.ToOffset(openingOffset);

        var dayOfWeek = this._schedule.Day;
        if ( ! end.IsLaterThan(this._schedule.Start.Hour, this._schedule.Start.Minute))
            dayOfWeek = dayOfWeek.Next();
        var daysUntilTarget = (dayOfWeek.ToDayOfWeek() - basis.DayOfWeek + 7) % 7;
        var firstClosing = new DateTimeOffset(basis.Year, basis.Month, basis.Day, end.Hour, end.Minute, 0, basis.Offset).AddDays(daysUntilTarget);
        if (firstClosing <= basis)
            firstClosing = firstClosing.AddDays(7);

        var firstOpening = new DateTimeOffset(firstClosing.Year, firstClosing.Month, firstClosing.Day,
            this._schedule.Start.Hour, this._schedule.Start.Minute, 0, firstClosing.Offset);
        if (firstOpening > firstClosing)
            firstOpening = firstOpening.AddDays(-1);
        
        this.Current = new Opening(firstOpening, firstClosing).SetOffset(_timeZone);
        return true;
    }

    private bool MoveNextByXthDayOfMonth()
    {
        if (this.Current != null)
        {
            this.Current = this.Current
                .AddMonths(1)
                .ResetDay()
                .RollToDay(this._schedule.Day)
                .AddDays(7 * (this._schedule.Interval.IntervalArgument - 1))
                .SetOffset(_timeZone);
            return true; 
        }
        
        var end = this._schedule.End ?? new Time
        {
            Hour = (ushort)((this._schedule.Start.Hour + 3) % 24),
            Minute = this._schedule.Start.Minute,
            TimeZone = this._schedule.Start.TimeZone
        };
        
        var basis = this._from;
        var offset = this._timeZone.GetUtcOffset(basis);

        var initialStart = new DateTimeOffset(basis.Year, basis.Month, 1,
            this._schedule.Start.Hour, this._schedule.Start.Minute, 0, offset); 
        var initialEnd = new DateTimeOffset(basis.Year, basis.Month, 1,
            end.Hour, end.Minute, 0, offset);
        if (initialStart > initialEnd)
            initialEnd = initialEnd.AddDays(1);

        this.Current = new Opening(initialStart, initialEnd)
            .RollToDay(this._schedule.Day)
            .AddDays(7 * (this._schedule.Interval.IntervalArgument - 1))
            .SetOffset(_timeZone);

        if (this.Current!.End < basis)
            return this.MoveNextByXthDayOfMonth();
        
        return true;
    }
    
    public void Reset() =>
        this.Current = null;

    public Opening Current { get; private set; }

    object IEnumerator.Current => Current;

    public void Dispose() { }
}