using System;

namespace FFXIVVenues.VenueModels.Tests.Helpers;

public static class DateOffsetGenerator
{
    public static DateTimeOffset GetDate(DayOfWeek day, int hour, int minute)
    {
        var testDate = new DateTime(2023, 11, 6, hour, minute, 0, DateTimeKind.Utc);
        testDate = testDate.AddDays(-(int)testDate.DayOfWeek).AddDays((int)day);
        var offset = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time").GetUtcOffset(testDate);
        testDate -= offset;
        return testDate;
    }

    public static DateTimeOffset GetDayOfMonth(int month, int occurence, DayOfWeek day, int hour, int minute)
    {
        var year = 2024;
        var testDate = new DateTime(year, month, 1, hour, minute, 0, DateTimeKind.Utc);

        if (occurence > 0)
        {
            testDate = testDate.AddDays((7 + (day - testDate.DayOfWeek)) % 7);
            testDate = testDate.AddDays(7 * (occurence - 1));
        }
        else if (occurence < 0)
        {
            testDate = new DateTime(year, month, DateTime.DaysInMonth(year, month), hour, minute, 0, DateTimeKind.Utc);
            testDate = testDate.AddDays(-((7 + (testDate.DayOfWeek - day)) % 7));
            testDate = testDate.AddDays(-7 * (Math.Abs(occurence) - 1));
        }

        var offset = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time").GetUtcOffset(testDate);
        testDate -= offset;
        return testDate;
    }
}