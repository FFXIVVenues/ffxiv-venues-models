using System;
using FFXIVVenues.VenueModels.Tests.Helpers;
using NUnit.Framework;

namespace FFXIVVenues.VenueModels.Tests.Schedule.Resolve.GivenAnOpeningOverMidnight;

public class WhenGivenTimeIsOutsideOpening
{

    [Test]
    [TestCase(DayOfWeek.Sunday, 22, 30)]
    [TestCase(DayOfWeek.Monday, 21, 59)]
    [TestCase(DayOfWeek.Tuesday, 1, 0)]
    [TestCase(DayOfWeek.Tuesday, 22, 30)]
    public void ThenResolveReturnsOpenFalse(DayOfWeek day, int hour, int minute)
    {
        var model = new VenueModels.Schedule
        {
            Day = 0,
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
        var result = model.Resolve(at).IsAt(at);

        Assert.IsFalse(result);
    }

}
