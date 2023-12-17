using System;
using NUnit.Framework;

namespace FFXIVVenues.VenueModels.Tests.Venue.Resolve;

public class GivenAVenueWithNoOpeningsOrOverrides
{
    [Test]
    public void ThenResolveReturnsNull()
    {
        // Arrange
        var venue = new VenueModels.Venue();
      
        // Act
        var result = venue.Resolve(DateTimeOffset.UtcNow);

        // Assert
        Assert.IsNull(result);
    }
}