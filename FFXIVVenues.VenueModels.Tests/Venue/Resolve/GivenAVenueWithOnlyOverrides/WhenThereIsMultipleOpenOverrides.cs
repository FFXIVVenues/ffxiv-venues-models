using System;
using FFXIVVenues.VenueModels.Tests.Helpers;
using NUnit.Framework;

namespace FFXIVVenues.VenueModels.Tests.Venue.Resolve.GivenAVenueWithOnlyOverrides;

public class WhenThereIsMultipleOpenOverrides
{
    [Test]
    public void ThenResolveReturnsTheEarliestOpeningResolved()
    {
        // Arrange
        var from = DateOffsetGenerator.GetDate(DayOfWeek.Tuesday, 15, 15);
        var venue = VenueGenerator.GenerateVenue();
        var expectedOverride = new ScheduleOverride
        {
            Open = true,
            Start = DateOffsetGenerator.GetDate(DayOfWeek.Tuesday, 17, 00),
            End = DateOffsetGenerator.GetDate(DayOfWeek.Tuesday, 19, 00),
        };
        var laterOverride = new ScheduleOverride
        {
            Open = true,
            Start = DateOffsetGenerator.GetDate(DayOfWeek.Thursday, 17, 00),
            End = DateOffsetGenerator.GetDate(DayOfWeek.Thursday, 19, 00),
        }; 
        venue.ScheduleOverrides.Add(expectedOverride);
        venue.ScheduleOverrides.Add(laterOverride);
      
        // Act
        var result = venue.Resolve(from);

        // Assert
        Assert.NotNull(result);
        Assert.AreEqual(expectedOverride.Start, result.Start);
        Assert.AreEqual(expectedOverride.End, result.End);
    }
}