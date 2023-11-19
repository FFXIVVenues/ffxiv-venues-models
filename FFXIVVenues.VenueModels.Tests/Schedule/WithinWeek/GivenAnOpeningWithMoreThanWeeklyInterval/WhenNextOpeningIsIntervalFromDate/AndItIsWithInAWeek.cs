using System;
using NUnit.Framework;

namespace FFXIVVenues.VenueModels.Tests.Schedule.WithinWeek.GivenAnOpeningWithMoreThanWeeklyInterval.WhenNextOpeningIsIntervalFromDate;

public class AndItIsWithInAWeek
{
    [Test]
    [TestCase(2, Day.Monday, (ushort)14, (ushort)00, -1)]
    [TestCase(2, Day.Thursday, (ushort)12, (ushort)00, -4)]
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
            Interval = new Interval
            {
                IntervalArgument = interval,
                IntervalFrom = startDate
            }
        };
    
        // Act
        var isWithinWeekOf = opening.WithinWeekOf(startDate.AddDays(daysFromIntervalFrom));

        // Assert
        Assert.IsTrue(isWithinWeekOf);
    }
}