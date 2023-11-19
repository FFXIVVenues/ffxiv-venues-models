using System;
using FFXIVVenues.VenueModels.Tests.Helpers;
using NUnit.Framework;

namespace FFXIVVenues.VenueModels.Tests.Schedule.IsAt.GivenAnOpeningInTheDay;

public class WhenGivenTimeIsWithinOpening
{
        
    [Test]
    [TestCase(DayOfWeek.Wednesday, 18, 0)]
    [TestCase(DayOfWeek.Wednesday, 18, 30)]
    [TestCase(DayOfWeek.Wednesday, 20, 59)]
    public void ThenIsAtReturnsTrue(DayOfWeek day, int hour, int minute)
    {
        var model = new VenueModels.Schedule
        {
            Day = Day.Wednesday,
            Start = new Time
            {
                Hour = 18,
                Minute = 0,
                NextDay = false,
                TimeZone = "Eastern Standard Time"
            },
            End = new Time
            {
                Hour = 21,
                Minute = 0,
                NextDay = false,
                TimeZone = "Eastern Standard Time"
            }
        };

        var at = DateOffsetGenerator.GetEstDate(day, hour, minute);
        var result = model.IsAt(at);
            
        Assert.IsTrue(result);
    }
}