using System;
using NUnit.Framework;

namespace FFXIVVenues.VenueModels.Tests.Schedule.WithinWeek.GivenAnOpeningWithMoreThanWeeklyInterval;

public class WhenNextOpeningIsWithinNextWeek
{
    [Test]
    [TestCase(1, Day.Monday, (ushort)14, (ushort)00, 2)]
    [TestCase(1, Day.Monday, (ushort)12, (ushort)00, 9)]
    [TestCase(2, Day.Tuesday, (ushort)9, (ushort)00, 8)]
    [TestCase(3, Day.Thursday, (ushort)10, (ushort)00, 15)]
    [TestCase(4, Day.Sunday, (ushort)19, (ushort)00, 25)]
    public void ThenWithInWeekReturnsTrue(int interval, Day day, ushort startHour, ushort startMinute, int daysFromIntervalFrom)
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
            Interval = new Interval
            {
                IntervalArgument = interval,
            }
        };
    
        // Act
        var isWithinWeekOf = opening.WithinWeekOf(startDate.AddDays(daysFromIntervalFrom));

        // Assert
        Assert.IsTrue(isWithinWeekOf);
    }
}