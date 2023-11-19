﻿using System;
using FFXIVVenues.VenueModels.Tests.Helpers;
using NUnit.Framework;

namespace FFXIVVenues.VenueModels.Tests.Schedule.IsAt.GivenAnOpeningOverMidnight;

public class WhenGivenTimeIsWithinOpening
{

    [Test]
    [TestCase(DayOfWeek.Monday, 22, 0)]
    [TestCase(DayOfWeek.Monday, 22, 30)]
    [TestCase(DayOfWeek.Tuesday, 0, 59)]
    public void ThenIsAtReturnsTrue(DayOfWeek day, int hour, int minute)
    {
        var model = new VenueModels.Schedule
        {
            Day = 0,
            Start = new Time
            {
                Hour = 22,
                Minute = 0,
                NextDay = false,
                TimeZone = "Eastern Standard Time"
            },
            End = new Time
            {
                Hour = 1,
                Minute = 0,
                NextDay = true,
                TimeZone = "Eastern Standard Time"
            }
        };

        var at = DateOffsetGenerator.GetEstDate(day, hour, minute);
        var result = model.IsAt(at);

        Assert.IsTrue(result);
    }

}