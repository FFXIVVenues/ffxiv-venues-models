using System;
using FFXIVVenues.VenueModels.Tests.Helpers;
using NUnit.Framework;

namespace FFXIVVenues.VenueModels.Tests.Venue.Resolve.GivenAVenueWithOverridedOpenings;

public class WhenTheFullScheduleIsOverridden
{
    [Test]
    public void ThenResolveReturnsTheNextSchedule()
    {
        // Arrange
        var from = DateOffsetGenerator.GetEstDate(DayOfWeek.Tuesday, 15, 15);
        var venue = VenueGenerator.GenerateVenue();
        var @override = new ScheduleOverride
        {
            Open = false,
            Start = from,
            End = from.AddDays(7),
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
        Assert.NotNull(result);
        Assert.AreEqual(schedules[0].Resolve(from).AddDays(7), result);
    }
}