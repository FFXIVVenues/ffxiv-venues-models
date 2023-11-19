using NUnit.Framework;

namespace FFXIVVenues.VenueModels.Tests.Schedule.ToUtc;

public class GivenAOpeningWithoutAnEndTime
{

    [Test]
    public void ThenToUtcReturnsAUtcEquivilentOpening()
    {
        var model = new VenueModels.Schedule
        {
            Day = Day.Wednesday,
            Start = new Time
            {
                Hour = 2,
                Minute = 0,
                NextDay = false,
                TimeZone = "America/Denver"
            }
        };
        
        var expected = new VenueModels.Schedule
        {
            Day = Day.Wednesday,
            Start = new Time
            {
                Hour = 9,
                Minute = 0,
                NextDay = false,
                TimeZone = "UTC"
            }
        };

        var utcModel = model.Utc;
        Assert.AreEqual(expected.Day, utcModel.Day);
        Assert.AreEqual(expected.Start.Hour, utcModel.Start.Hour);
        Assert.AreEqual(expected.Start.Minute, utcModel.Start.Minute);
        Assert.AreEqual(expected.Start.NextDay, utcModel.Start.NextDay);
        Assert.AreEqual(expected.Start.TimeZone, utcModel.Start.TimeZone);
        Assert.IsNull(expected.End);
    }
}