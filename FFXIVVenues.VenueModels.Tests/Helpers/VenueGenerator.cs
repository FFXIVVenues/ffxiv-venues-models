using System;
using System.Collections.Generic;
using System.Linq;

namespace FFXIVVenues.VenueModels.Tests.Helpers;

public static class VenueGenerator
{
    private static Random random = new Random();

    public static VenueModels.Venue GenerateVenue()
    {
        var managers = GetRandomManagerList();
        var tags = GetRandomTagList();

        return new VenueModels.Venue
        {
            Id = Guid.NewGuid().ToString(),
            Name = $"Test Venue {random.Next(1000)}",
            BannerUri = new Uri("http://example.com"),
            Added = DateTimeOffset.UtcNow,
            Description = new List<string> { "Test Description" },
            Location = LocationGenerator.GenerateApartment(),
            Website = new Uri("https://example.carrd.co/"),
            Discord = new Uri($"https://discord.gg/{GetRandomAlphanumericString(8)}"),
            OpenHouse = random.Next(2) == 1,
            Sfw = random.Next(2) == 1,
            Schedule = new List<VenueModels.Schedule> {  },
            ScheduleOverrides = new List<ScheduleOverride> {  },
            Notices = new List<Notice> {  },
            Managers = managers,
            Tags = tags,
            Approved = random.Next(2) == 1,
            LastModified = DateTimeOffset.UtcNow,
            MareCode = "Test",
            MarePassword = "Test",
        };
    }

    private static string GetRandomAlphanumericString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    private static List<string> GetRandomManagerList()
    {
        var managerCount = random.Next(1, 10);
        var managers = new List<string>();

        for (int i = 0; i < managerCount; i++)
        {
            managers.Add($"Manager{i + 1}");
        }

        return managers;
    }

    private static List<string> GetRandomTagList()
    {
        var tagCount = random.Next(1, 10);
        var tags = new List<string>();

        for (int i = 0; i < tagCount; i++)
        {
            tags.Add($"Tag{i + 1}");
        }

        return tags;
    }
}