using System;
using NUnit.Framework;

namespace FFXIVVenues.VenueModels.Tests.Schedule.WithinWeek.GivenAnOpeningWithMoreThanWeeklyInterval.WhenNextOpeningIsIntervalFromDate;

public class AndIsFurtherThanAWeekAway {

    [Test]
    [TestCase(2, Day.Monday, (ushort)14, (ushort)00, -7)]
    [TestCase(3, Day.Thursday, (ushort)12, (ushort)00, -16)]
    public void ThenWithInWeekReturnsFalse(int interval, Day day, ushort startHour, ushort startMinute, int daysFromIntervalFrom)
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
        Assert.IsFalse(isWithinWeekOf);
    } 
}
