using System;
using FFXIVVenues.VenueModels.Tests.Helpers;
using NUnit.Framework;

namespace FFXIVVenues.VenueModels.Tests.Venue.Resolve.GivenAVenueWithOnlyOverrides;

public class WhenThereIsOneOpenOverride
{
    [Test]
    public void ThenResolveReturnsThatOpeningResolved()
    {
        // Arrange
        var from = DateOffsetGenerator.GetEstDate(DayOfWeek.Sunday, 15, 15);
        var venue = VenueGenerator.GenerateVenue();
        var expectedOverride = new ScheduleOverride
        {
            Open = true,
            Start = DateOffsetGenerator.GetEstDate(DayOfWeek.Sunday, 17, 00),
            End = DateOffsetGenerator.GetEstDate(DayOfWeek.Sunday, 19, 00),
        };
        venue.ScheduleOverrides.Add(expectedOverride);
      
        // Act
        var result = venue.Resolve(from);

        // Assert
        Assert.NotNull(result);
        Assert.AreEqual(expectedOverride.Start, result.Start);
        Assert.AreEqual(expectedOverride.End, result.End);
    }
}