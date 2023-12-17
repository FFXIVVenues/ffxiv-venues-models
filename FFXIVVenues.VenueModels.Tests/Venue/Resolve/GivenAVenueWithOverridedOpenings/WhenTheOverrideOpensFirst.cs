using System;
using FFXIVVenues.VenueModels.Tests.Helpers;
using NUnit.Framework;

namespace FFXIVVenues.VenueModels.Tests.Venue.Resolve.GivenAVenueWithOverridedOpenings;

public class WhenTheOverrideOpensFirst
{
    [Test]
    public void ThenResolveReturnsTheOverride()
    {
        // Arrange
        var from = DateOffsetGenerator.GetEstDate(DayOfWeek.Tuesday, 15, 15);
        var venue = VenueGenerator.GenerateVenue();
        var expectedOverride = new ScheduleOverride
        {
            Open = true,
            Start = DateOffsetGenerator.GetEstDate(DayOfWeek.Tuesday, 17, 00),
            End = DateOffsetGenerator.GetEstDate(DayOfWeek.Tuesday, 19, 00),
        };; 
        venue.ScheduleOverrides.Add(expectedOverride);
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
        Assert.NotNull(result);
        Assert.AreEqual(expectedOverride.Start, result.Start);
        Assert.AreEqual(expectedOverride.End, result.End);
    }
}