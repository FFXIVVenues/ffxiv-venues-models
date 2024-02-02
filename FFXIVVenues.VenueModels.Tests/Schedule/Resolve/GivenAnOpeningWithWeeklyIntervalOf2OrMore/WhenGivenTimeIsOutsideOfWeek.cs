using System;
using FFXIVVenues.VenueModels.Tests.Helpers;
using NUnit.Framework;

namespace FFXIVVenues.VenueModels.Tests.Schedule.Resolve.GivenAnOpeningWithWeeklyIntervalOf2OrMore;

public class WhenGivenTimeIsOutsideOfWeek
    {
        [Test]
        public void ThenResolveReturnsOpenFalse()
        {
            var at = DateOffsetGenerator.GetDate(DayOfWeek.Wednesday, 22, 0);
            var model = new VenueModels.Schedule
            {
                Day = Day.Wednesday,
                Commencing = at.AddDays(-7),
                Interval = new Interval
                {
                    IntervalType = IntervalType.EveryXWeeks,
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
            
            var result = model.Resolve(at).IsAt(at);
            
            Assert.IsFalse(result);
        }
    }
