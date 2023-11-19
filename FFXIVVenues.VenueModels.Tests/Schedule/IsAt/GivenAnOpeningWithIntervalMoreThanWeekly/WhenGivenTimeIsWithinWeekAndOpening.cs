﻿using System;
using FFXIVVenues.VenueModels.Tests.Helpers;
using NUnit.Framework;

namespace FFXIVVenues.VenueModels.Tests.Schedule.IsAt.GivenAnOpeningWithIntervalMoreThanWeekly;

public class WhenGivenTimeIsWithinWeekAndOpening
{
    [Test]
    public void ThenIsAtReturnsTrue()
    {
        var at = DateOffsetGenerator.GetEstDate(DayOfWeek.Wednesday, 22, 0);
        var model = new VenueModels.Schedule
        {
            Day = Day.Wednesday,
            Interval = new Interval
            {
                IntervalArgument = 2,
                IntervalFrom = at.AddDays(-14)
            },
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

        var result = model.IsAt(at);
        
        Assert.IsTrue(result);
    }
}