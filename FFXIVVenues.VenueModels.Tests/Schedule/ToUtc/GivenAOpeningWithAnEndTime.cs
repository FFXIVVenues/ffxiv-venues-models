using System;
using NUnit.Framework;

namespace FFXIVVenues.VenueModels.Tests.Schedule.ToUtc;

public class GivenAOpeningWithAnEndTime
{

    [Test]
    [TestCase(Day.Monday, (ushort)22, (ushort)30, false, (ushort)01, (ushort)00, true, "Europe/Chisinau",
        Day.Monday, (ushort)20, (ushort)30, false, (ushort)23, (ushort)00, false,
        Description = "Backwards to UTC, closing over midnight boundary")]
    [TestCase(Day.Tuesday, (ushort)21, (ushort)00, false, (ushort)23, (ushort)00, false, "America/Denver",
        Day.Wednesday, (ushort)04, (ushort)00, false, (ushort)06, (ushort)00, false,
        Description = "Forward to UTC, opening and closing over midnight boundary")]
    [TestCase(Day.Friday, (ushort)0, (ushort)00, false, (ushort)04, (ushort)30, true, "Europe/Chisinau",
        Day.Thursday, (ushort)22, (ushort)00, false, (ushort)02, (ushort)30, true,
        Description = "Backwards to UTC, opening over midnight boundary")]
    [TestCase(Day.Tuesday, (ushort)15, (ushort)45, false, (ushort)19, (ushort)00, false, "America/Denver",
        Day.Tuesday, (ushort)22, (ushort)45, false, (ushort)02, (ushort)00, true,
        Description = "Forward to UTC, closing over midnight boundary")]
    [TestCase(Day.Saturday, (ushort)00, (ushort)00, false, (ushort)01, (ushort)00, true, "Europe/Chisinau",
        Day.Friday, (ushort)22, (ushort)00, false, (ushort)23, (ushort)00, false,
        Description = "Backwards to UTC, opening and closing over midnight boundary")]
    [TestCase(Day.Tuesday, (ushort)10, (ushort)00, false, (ushort)12, (ushort)00, false, "America/Denver",
        Day.Tuesday, (ushort)17, (ushort)00, false, (ushort)19, (ushort)00, false,
        Description = "Forward to UTC, all within day")]
    [TestCase(Day.Wednesday, (ushort)02, (ushort)00, true, (ushort)04, (ushort)00, true, "America/Denver",
        Day.Thursday, (ushort)09, (ushort)00, false, (ushort)11, (ushort)00, false,
        Description = "Forward to UTC, all within next day")]
    public void ThenToUtcReturnsAUtcEquivilentOpening(
        Day day, ushort openHour, ushort openMinute, bool openNextDay,
        ushort closeHour, ushort closeMinute, bool closeNextDay, string timeZone,
        Day expectedDay, ushort expectedOpenHour, ushort expectedOpenMinute, bool expectedOpenNextDay,
        ushort expectedCloseHour, ushort expectedCloseMinute, bool expectedCloseNextDay)
    {
        var model = new VenueModels.Schedule
        {
            Day = day,
            Start = new Time
            {
                Hour = openHour,
                Minute = openMinute,
                NextDay = openNextDay,
                TimeZone = timeZone
            },
            End = new Time
            {
                Hour = closeHour,
                Minute = closeMinute,
                NextDay = closeNextDay,
                TimeZone = timeZone
            }
        };

        var expected = new VenueModels.Schedule
        {
            Day = expectedDay,
            Start = new Time
            {
                Hour = expectedOpenHour,
                Minute = expectedOpenMinute,
                NextDay = expectedOpenNextDay,
                TimeZone = "UTC"
            },
            End = new Time
            {
                Hour = expectedCloseHour,
                Minute = expectedCloseMinute,
                NextDay = expectedCloseNextDay,
                TimeZone = "UTC"
            }
        };

        var dlsOffset = 0;
        if (DateTime.UtcNow.IsDaylightSavingTime())
            dlsOffset = -1;

        var utcModel = model.Utc;
        Assert.AreEqual(expected.Day, utcModel.Day);
        Assert.AreEqual(expected.Start.Hour, utcModel.Start.Hour - dlsOffset);
        Assert.AreEqual(expected.Start.Minute, utcModel.Start.Minute);
        Assert.AreEqual(expected.Start.NextDay, utcModel.Start.NextDay);
        Assert.AreEqual(expected.Start.TimeZone, utcModel.Start.TimeZone);
        Assert.AreEqual(expected.End.Hour, utcModel.End.Hour - dlsOffset);
        Assert.AreEqual(expected.End.Minute, utcModel.End.Minute);
        Assert.AreEqual(expected.End.NextDay, utcModel.End.NextDay);
        Assert.AreEqual(expected.End.TimeZone, utcModel.End.TimeZone);
    }

}