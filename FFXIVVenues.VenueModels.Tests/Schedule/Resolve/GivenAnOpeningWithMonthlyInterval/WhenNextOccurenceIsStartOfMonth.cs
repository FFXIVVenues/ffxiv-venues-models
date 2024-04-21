using System;
using FFXIVVenues.VenueModels.Tests.Helpers;
using NUnit.Framework;

namespace FFXIVVenues.VenueModels.Tests.Schedule.Resolve.GivenAnOpeningWithMonthlyInterval;

public class WhenNextOccurenceIsStartOfMonth
{

    [Test]
    public void ThenResolveReturnsLater()
    {
        var at = DateOffsetGenerator.GetDayOfMonth(2, 1, DayOfWeek.Thursday, 22, 30);
        var model = new VenueModels.Schedule
        {
            Day = Day.Thursday,
            Commencing = DateOffsetGenerator.GetDayOfMonth(1, 1, DayOfWeek.Monday, 21, 0),
            Interval = new Interval { IntervalType = IntervalType.EveryXthDayOfTheMonth, IntervalArgument = 2 },
            Start = new Time { Hour = 21, Minute = 0, NextDay = false, TimeZone = "Eastern Standard Time" },
            End = new Time { Hour = 0, Minute = 0, NextDay = true, TimeZone = "Eastern Standard Time" }
        };
            
        var result = model.Resolve(at);

        var expected = DateOffsetGenerator.GetDayOfMonth(2, 2, DayOfWeek.Thursday, 21, 0);
        Assert.AreEqual(expected, result.Start, "The resulting start date is not as expected.");
        Assert.AreEqual(expected.AddHours(3), result.End, "The resulting end date is not as expected.");
    }

}