using System;

namespace FFXIVVenues.VenueModels;

public record Opening(DateTimeOffset Start, DateTimeOffset End)
{
    public Opening AddDays(int days)
    {
        var newStart = this.Start.AddDays(days);
        var newEnd = this.End.AddDays(days);
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
}