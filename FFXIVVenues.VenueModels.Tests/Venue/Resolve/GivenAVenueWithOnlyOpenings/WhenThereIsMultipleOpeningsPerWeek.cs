using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using FFXIVVenues.VenueModels.Tests.Helpers;
using NUnit.Framework;

namespace FFXIVVenues.VenueModels.Tests.Venue.Resolve.GivenAVenueWithOnlyOpenings;

public class WhenThereIsMultipleOpeningsPerWeek
{
    [Test]
    public void ThenResolveReturnsThatOpeningResolved()
    {
        // Arrange
        var from = DateOffsetGenerator.GetEstDate(DayOfWeek.Tuesday, 15, 15);
        var venue = VenueGenerator.GenerateVenue();
        var expectedSchedules = new[]
        {
            new VenueModels.Schedule
            {
                Day = Day.Monday,
                Start = new() { Hour = 22, Minute = 0, TimeZone = "Eastern Standard Time" },
                End = new() { Hour = 1, Minute = 0, TimeZone = "Eastern Standard Time" },
            },
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
        venue.Schedule.AddRange(expectedSchedules);
      
        // Act
        var result = venue.Resolve(from);

        // Assert
        Assert.AreEqual(expectedSchedules[1].Resolve(from), result);
    }
}