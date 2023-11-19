using System;

namespace FFXIVVenues.VenueModels;

public class Interval
{
    public IntervalType IntervalType { get; set; }
    public int IntervalArgument { get; set; } = 1;
    public DateTimeOffset IntervalFrom { get; set; }
}

public enum IntervalType
{
    EveryXWeeks,
    XDayOfEveryMonth
}