using System;
using FFXIVVenues.VenueModels.Tests.Helpers;
using NUnit.Framework;

namespace FFXIVVenues.VenueModels.Tests.Schedule.Resolve.GivenAnOpeningAtMidnight;

public class WhenGivenTimeIsWithinOpening
{

    [Test]
    [TestCase(DayOfWeek.Tuesday, 0, 0)]
    [TestCase(DayOfWeek.Tuesday, 0, 30)]
    [TestCase(DayOfWeek.Tuesday, 2, 59)]
    public void ThenResolveReturnsOpenTrue(DayOfWeek day, int hour, int minute)
    {
        var model = new VenueModels.Schedule
        {
            Day = Day.Tuesday,
            Start = new Time
            {
                Hour = 0,
                Minute = 0,
                TimeZone = "Eastern Standard Time"
            },
            End = new Time
            {
                Hour = 3,
                Minute = 0,
                TimeZone = "Eastern Standard Time"
            }
        };

        var at = DateOffsetGenerator.GetEstDate(day, hour, minute);
        var result = model.Resolve(at).IsAt(at);
            
        Assert.IsTrue(result);
    }
}