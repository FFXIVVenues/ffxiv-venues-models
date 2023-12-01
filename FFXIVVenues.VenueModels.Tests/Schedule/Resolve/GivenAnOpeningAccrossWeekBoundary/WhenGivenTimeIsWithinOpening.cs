using System;
using FFXIVVenues.VenueModels.Tests.Helpers;
using NUnit.Framework;

namespace FFXIVVenues.VenueModels.Tests.Schedule.Resolve.GivenAnOpeningAccrossWeekBoundary;

public class WhenGivenTimeIsWithinOpening
{
    [Test]
    [TestCase(DayOfWeek.Sunday, 22, 0)]
    [TestCase(DayOfWeek.Sunday, 22, 30)]
    [TestCase(DayOfWeek.Monday, 0, 59)]
    public void ThenResolveReturnsOpenTrue(DayOfWeek day, int hour, int minute)
    {
        var model = new VenueModels.Schedule
        {
            Day = Day.Sunday,
            Start = new Time
            {
                Hour = 22,
                Minute = 0,
                NextDay = false,
                TimeZone = "Eastern Standard Time"
            },
            End = new Time
            {
                Hour = 1,
                Minute = 0,
                NextDay = true,
                TimeZone = "Eastern Standard Time"
            }
        };
            
        var at = DateOffsetGenerator.GetEstDate(day, hour, minute);
        var result = model.Resolve(at).IsOpen;
            
        Assert.IsTrue(result);
    }
}