using System;
using FFXIVVenues.VenueModels.Tests.Helpers;
using NUnit.Framework;

namespace FFXIVVenues.VenueModels.Tests.Venue.Resolve.GivenAVenueWithOverridedOpenings;

public class WhenTheFirstScheduleIsSplitByOverride
{
    [Test]
    public void ThenResolveReturnsTheFirstHalfOfTheFirstSchedule()
    {
        // Arrange
        var from = DateOffsetGenerator.GetDate(DayOfWeek.Tuesday, 15, 15);
        var venue = VenueGenerator.GenerateVenue();
        var @override = new ScheduleOverride
        {
            Open = false,
            Start = DateOffsetGenerator.GetDate(DayOfWeek.Wednesday, 21, 00),
            End = DateOffsetGenerator.GetDate(DayOfWeek.Thursday, 22, 00),
        };
        venue.ScheduleOverrides.Add(@override);
        var schedules = new[]
        {
            new VenueModels.Schedule
            {
                Day = Day.Wednesday,
                Start = new() { Hour = 20, Minute = 0, TimeZone = "Eastern Standard Time" },
                End = new() { Hour = 23, Minute = 0, TimeZone = "Eastern Standard Time" },
            },
            new VenueModels.Schedule
            {
                Day = Day.Friday,
                Start = new() { Hour = 20, Minute = 0, TimeZone = "Eastern Standard Time" },
                End = new() { Hour = 23, Minute = 0, TimeZone = "Eastern Standard Time" },
            }
        };
        venue.Schedule.AddRange(schedules);
        
        // Act
        var result = venue.Resolve(from);

        // Assert
        var resolve = schedules[0].Resolve(from);
        var expected = resolve with { End = resolve.Start.AddHours(1) };
        
        Assert.NotNull(result);
        Assert.AreEqual(expected, result);
    }
}