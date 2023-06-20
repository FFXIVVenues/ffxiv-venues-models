using FFXIVVenues.VenueModels;
using NUnit.Framework;
using System;

namespace FFXIVVenues.VenueModels.Tests
{
    public partial class OpeningTests
    {
        [Test]
        [TestCase(DayOfWeek.Sunday, 22, 30, false)]
        [TestCase(DayOfWeek.Monday, 21, 59, false)]
        [TestCase(DayOfWeek.Monday, 22, 0, true)]
        [TestCase(DayOfWeek.Monday, 22, 30, true)]
        [TestCase(DayOfWeek.Tuesday, 0, 59, true)]
        [TestCase(DayOfWeek.Tuesday, 1, 0, false)]
        [TestCase(DayOfWeek.Tuesday, 22, 30, false)]
        public void OpenAccrossMidnight(DayOfWeek day, int hour, int minute, bool shouldBeOpen)
        {
            var model = new Opening
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
            var testDate = GetESTDate(day, hour, minute);
            Assert.AreEqual(shouldBeOpen, model.IsAt(testDate));
        }

        [Test]
        [TestCase(DayOfWeek.Monday, 0, 0, false)]
        [TestCase(DayOfWeek.Monday, 23, 59, false)]
        [TestCase(DayOfWeek.Tuesday, 0, 0, true)]
        [TestCase(DayOfWeek.Tuesday, 0, 30, true)]
        [TestCase(DayOfWeek.Tuesday, 2, 59, true)]
        [TestCase(DayOfWeek.Tuesday, 3, 0, false)]
        [TestCase(DayOfWeek.Tuesday, 23, 59, false)]
        public void WhenOpenAtMidnight(DayOfWeek day, int hour, int minute, bool shouldBeOpen)
        {
            var model = new Opening
            {
                Day = 0,
                Start = new Time
                {
                    Hour = 0,
                    Minute = 0,
                    NextDay = true,
                    TimeZone = "Eastern Standard Time"
                },
                End = new Time
                {
                    Hour = 3,
                    Minute = 0,
                    NextDay = true,
                    TimeZone = "Eastern Standard Time"
                }
            };

            var testDate = GetESTDate(day, hour, minute);
            Assert.AreEqual(shouldBeOpen, model.IsAt(testDate));
        }

        [Test]
        [TestCase(DayOfWeek.Tuesday, 18, 00, false)]
        [TestCase(DayOfWeek.Wednesday, 17, 59, false)]
        [TestCase(DayOfWeek.Wednesday, 18, 0, true)]
        [TestCase(DayOfWeek.Wednesday, 18, 30, true)]
        [TestCase(DayOfWeek.Wednesday, 20, 59, true)]
        [TestCase(DayOfWeek.Wednesday, 21, 0, false)]
        [TestCase(DayOfWeek.Thursday, 20, 59, false)]
        public void WhenOpenInDay(DayOfWeek day, int hour, int minute, bool shouldBeOpen)
        {
            var model = new Opening
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

            var testDate = GetESTDate(day, hour, minute);
            Assert.AreEqual(shouldBeOpen, model.IsAt(testDate));
        }

        [Test]
        [TestCase(DayOfWeek.Saturday, 22, 30, false)]
        [TestCase(DayOfWeek.Sunday, 21, 59, false)]
        [TestCase(DayOfWeek.Sunday, 22, 0, true)]
        [TestCase(DayOfWeek.Sunday, 22, 30, true)]
        [TestCase(DayOfWeek.Monday, 0, 59, true)]
        [TestCase(DayOfWeek.Monday, 1, 0, false)]
        [TestCase(DayOfWeek.Monday, 22, 30, false)]
        public void OpenAccrossWeekBoundary(DayOfWeek day, int hour, int minute, bool shouldBeOpen)
        {
            var model = new Opening
            {
                Day = Day.Sunday,
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
            var testDate = GetESTDate(day, hour, minute);
            Assert.AreEqual(shouldBeOpen, model.IsAt(testDate));
        }


        private static DateTime GetESTDate(DayOfWeek day, int hour, int minute)
        {
            var testDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, hour, minute, 0, DateTimeKind.Utc);
            testDate = testDate.AddDays(-(int)testDate.DayOfWeek).AddDays((int)day);
            var offset = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time").GetUtcOffset(testDate);
            testDate -= offset;
            return testDate;
        }

    }
}
