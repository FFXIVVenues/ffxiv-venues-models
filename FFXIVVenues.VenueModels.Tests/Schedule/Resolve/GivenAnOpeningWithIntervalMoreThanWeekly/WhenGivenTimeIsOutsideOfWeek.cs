﻿using System;
using FFXIVVenues.VenueModels.Tests.Helpers;
using NUnit.Framework;

namespace FFXIVVenues.VenueModels.Tests.Schedule.Resolve.GivenAnOpeningWithIntervalMoreThanWeekly;

public class WhenGivenTimeIsOutsideOfWeek
    {
        [Test]
        public void ThenResolveReturnsOpenFalse()
        {
            var at = DateOffsetGenerator.GetEstDate(DayOfWeek.Wednesday, 22, 0);
            var model = new VenueModels.Schedule
            {
                Day = Day.Wednesday,
                From = at.AddDays(-7),
                Interval = new Interval
                {
                    IntervalArgument = 2,
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
            
            var result = model.Resolve(at).IsOpen;
            
            Assert.IsFalse(result);
        }
    }