using System;
using FFXIVVenues.VenueModels.Tests.Helpers;
using NUnit.Framework;

namespace FFXIVVenues.VenueModels.Tests.Schedule.Resolve.GivenAnOpeningInTheDay;

public class WhenGivenTimeOutsideOpening
    {
        
        [Test]
        [TestCase(DayOfWeek.Tuesday, 18, 00)]
        [TestCase(DayOfWeek.Wednesday, 17, 59)]
        [TestCase(DayOfWeek.Wednesday, 21, 0)]
        [TestCase(DayOfWeek.Thursday, 20, 59)]
        public void ThenResolveReturnsOpenFalse(DayOfWeek day, int hour, int minute)
        {
            var model = new VenueModels.Schedule
            {
                Day = Day.Wednesday,
                Start = new Time
                {
                    Hour = 18,
                    Minute = 0,
                    NextDay = false,
                    TimeZone = "Eastern Standard Time"
                },
                End = new Time
                {
                    Hour = 21,
                    Minute = 0,
                    NextDay = false,
                    TimeZone = "Eastern Standard Time"
                }
            };

            var at = DateOffsetGenerator.GetEstDate(day, hour, minute);
            var result = model.Resolve(at).IsAt(at);
            
            Assert.IsFalse(result);
        }
    }
