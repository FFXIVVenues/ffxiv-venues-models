using System;

namespace FFXIVVenues.VenueModels;

public class Interval
{
    public IntervalType IntervalType { get; set; }
    public int IntervalArgument { get; set; } = 1;
}

public enum IntervalType
{
    EveryXWeeks = 0,
    EveryXthDayOfTheMonth = 1, 
}