using System;
using FFXIVVenues.VenueModels.Tests.Helpers;
using NUnit.Framework;

namespace FFXIVVenues.VenueModels.Tests.Schedule.IsAt.GivenAnOpeningAtMidnight;

public class WhenGivenTimeIsOutsideOpening
    {

        [Test]
        [TestCase(DayOfWeek.Monday, 0, 0)]
        [TestCase(DayOfWeek.Monday, 23, 59)]
        [TestCase(DayOfWeek.Tuesday, 3, 0)]
        [TestCase(DayOfWeek.Tuesday, 23, 59)]
        public void ThenIsAtReturnsFalse(DayOfWeek day, int hour, int minute)
        {
            var model = new VenueModels.Schedule
            {
                Day = Day.Tuesday,
                Start = new Time
                {
                    Hour = 0,
                    Minute = 0,
                    TimeZone = "Eastern Standard Time"
                },
                End = new Time
                {
                    Hour = 3,
                    Minute = 0,
                    TimeZone = "Eastern Standard Time"
                }
            };

            var at = DateOffsetGenerator.GetEstDate(day, hour, minute);
            var result = model.IsAt(at);
            
            Assert.IsFalse(result);
        }
    }
