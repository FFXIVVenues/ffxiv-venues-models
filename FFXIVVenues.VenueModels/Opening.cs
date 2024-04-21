using System;

namespace FFXIVVenues.VenueModels;

public record Opening(DateTimeOffset Start, DateTimeOffset End)
{
    public bool IsNow => this.IsAt(DateTimeOffset.UtcNow);
    public bool IsWithinWeek => this.IsWithinWeekOf(DateTimeOffset.Now);

    public bool IsAt(DateTimeOffset at) =>
        at >= Start && at < End;
    
    public bool IsWithinWeekOf(DateTimeOffset at) =>
        Start < at.AddDays(7);
    
    public Opening AddDays(int days)
    {
        var newStart = this.Start.AddDays(days);
        var newEnd = this.End.AddDays(days);
        return new Opening(newStart, newEnd);
    }
    
    public Opening RemoveDays(int days)
    {
        var newStart = this.Start.AddDays(-days);
        var newEnd = this.End.AddDays(-days);
        return new Opening(newStart, newEnd);
    }

    public Opening AddMonths(int months)
    {
        var newStart = this.Start.AddMonths(months);
        var newEnd = this.End.AddMonths(months);
        return new Opening(newStart, newEnd);
    }

    public Opening ResetDay()
    {
        var resetSpan = (this.Start.Day - 1);
        var newStart = this.Start.AddDays(-resetSpan);
        var newEnd = this.End.AddDays(-resetSpan);
        return new Opening(newStart, newEnd);
    }

    public Opening MaxDay()
    {
        var days = DateTime.DaysInMonth(this.Start.Year, this.Start.Month);
        var resetSpan = days - this.Start.Day;
        var newStart = this.Start.AddDays(resetSpan);
        var newEnd = this.End.AddDays(resetSpan);
        return new Opening(newStart, newEnd);
    }

    public Opening RollToDay(Day day)
    {
        var dayOfWeek = day.ToDayOfWeek();
        var rollForward = ((int)dayOfWeek - (int)this.Start.DayOfWeek + 7) % 7;
            
        var newStart = this.Start.AddDays(rollForward);
        var newEnd = this.End.AddDays(rollForward);
        if (newStart > newEnd)
            newEnd = newEnd.AddDays(1);
        
        return new Opening(newStart, newEnd);
    }

    public Opening RollBackToDay(Day day)
    {
        var dayOfWeek = day.ToDayOfWeek();
        var rollBackwards = ((int)this.Start.DayOfWeek - (int)dayOfWeek + 7) % 7;

        var newStart = this.Start.AddDays(-rollBackwards);
        var newEnd = this.End.AddDays(-rollBackwards);
        if (newStart > newEnd)
            newEnd = newEnd.AddDays(1);

        return new Opening(newStart, newEnd);
    }
    
    public Opening SetOffset(TimeZoneInfo timeZone)
    {
        var startOffset = timeZone.GetUtcOffset(this.Start);
        var newStart = new DateTimeOffset(this.Start.Year, this.Start.Month, this.Start.Day, this.Start.Hour, this.Start.Minute, 0, startOffset);
        var endOffset = timeZone.GetUtcOffset(this.End);
        var newEnd = new DateTimeOffset(this.End.Year, this.End.Month, this.End.Day, this.End.Hour, this.End.Minute, 0, endOffset);
        return new Opening(newStart, newEnd);
    }

    public Opening[] Truncate(DateTimeOffset startRemove, DateTimeOffset endRemove)
    {
        if (endRemove <= this.Start || startRemove >= this.End)
            return new[] { this };
        else if (startRemove <= this.Start && endRemove >= this.End)
            return Array.Empty<Opening>();
        else if (startRemove <= this.Start)
            return new [] { new Opening(endRemove, this.End) };
        else if (endRemove >= this.End)
            return new [] { new Opening(this.Start, startRemove) };
        else
            return new [] { 
                new Opening(this.Start, startRemove), 
                new Opening(endRemove, this.End)
            };
    }
    
}