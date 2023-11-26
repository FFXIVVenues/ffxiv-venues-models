using System;
using NUnit.Framework;

namespace FFXIVVenues.VenueModels.Tests.Schedule.WithinWeek.GivenAnOpeningWithMoreThanWeeklyInterval;

public class WhenNextOpeningIsNow
{
        
    [Test]
    [TestCase(2, Day.Monday, (ushort)14, (ushort)00, 0)]
    [TestCase(3, Day.Wednesday, (ushort)12, (ushort)30, 0)]
    public void ThenWithinWeekOfReturnsTrue(int interval, Day day, ushort startHour, ushort startMinute, int daysFromIntervalFrom)
    {
        var startDate = new DateTimeOffset(DateTimeOffset.UtcNow.Year, DateTimeOffset.UtcNow.Month, DateTimeOffset.UtcNow.Day, startHour, startMinute, 0, new TimeSpan());
        var daysUntilTargetDay = ((int)day - (int)startDate.DayOfWeek + 7) % 7;
        startDate = startDate.AddDays(-daysUntilTargetDay);
        
        // Arrange
        var opening = new VenueModels.Schedule
        {
            Day = day,
            Start = new Time {  Hour = startHour, Minute = startMinute, TimeZone = "UTC"},
            End = new Time { Hour = (ushort) ((startHour+2)%24), Minute = startMinute, TimeZone = "UTC"},
            From = startDate,
            Interval = new Interval {
                IntervalArgument = interval,
            }
        };

        // Act
        var isWithinWeekOf = opening.WithinWeekOf(startDate.AddDays(daysFromIntervalFrom));

        // Assert
        Assert.IsTrue(isWithinWeekOf);
    }
        
}