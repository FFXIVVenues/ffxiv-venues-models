using System;

namespace FFXIVVenues.VenueModels.Tests.Helpers;

public static class DateOffsetGenerator
{
    public static DateTimeOffset GetEstDate(DayOfWeek day, int hour, int minute)
    {
        var testDate = new DateTime(2023, 11, 6, hour, minute, 0, DateTimeKind.Utc);
        testDate = testDate.AddDays(-(int)testDate.DayOfWeek).AddDays((int)day);
        var offset = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time").GetUtcOffset(testDate);
        testDate -= offset;
        return testDate;
    }
}