using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using FFXIVVenues.VenueModels.Tests.Helpers;
using NUnit.Framework;

namespace FFXIVVenues.VenueModels.Tests.Venue.Resolve.GivenAVenueWithOnlyOpenings;

public class WhenThereIsOneOpeningPerWeek
{
    [Test]
    public void ThenResolveReturnsThatOpeningResolved()
    {
        // Arrange
        var from = DateOffsetGenerator.GetEstDate(DayOfWeek.Sunday, 15, 15);
        var venue = VenueGenerator.GenerateVenue();
        var expectedSchedule = new VenueModels.Schedule()
        {
            Day = Day.Monday,
            Start = new() { Hour = 22, Minute = 0, TimeZone = "Eastern Standard Time" },
            End = new() { Hour = 1, Minute = 0, TimeZone = "Eastern Standard Time" },
        };
        venue.Schedule.Add(expectedSchedule);
      
        // Act
        var result = venue.Resolve(from);

        // Assert
        Assert.AreEqual(expectedSchedule.Resolve(from), result);
    }
}