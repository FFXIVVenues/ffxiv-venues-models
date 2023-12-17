namespace FFXIVVenues.VenueModels.Tests.Helpers;

using System;
using FFXIVVenues.VenueModels;

public static class LocationGenerator
{
    private static Random random = new Random();

    public static Location GenerateHouse()
    {
        var plot = (ushort)random.Next(60);
        return new Location
        {
            DataCenter = new [] { "Aether", "Crystal", "Chaos", "Light" }[random.Next(0, 3)],
            World = new [] { "Gilgamesh", "Jenova", "Balmung", "Zaleria" }[random.Next(0, 3)],
            District = new [] { "Goblet", "Lavender Beds", "Foundation", "Mist" }[random.Next(0, 3)],
            Ward = (ushort)random.Next(1, 30),
            Plot = plot,
            Apartment = 0,
            Room = 0,
            Subdivision = plot > 30
        };
    }
    
    public static Location GenerateApartment()
    {
        return new Location
        {
            DataCenter = new [] { "Aether", "Crystal", "Chaos", "Light" }[random.Next(0, 3)],
            World = new [] { "Gilgamesh", "Jenova", "Balmung", "Zaleria" }[random.Next(0, 3)],
            District = new [] { "Goblet", "Lavender Beds", "Foundation", "Mist" }[random.Next(0, 3)],
            Ward = (ushort)random.Next(1, 30),
            Plot = 0,
            Apartment = (ushort)random.Next(500),
            Room = 0,
            Subdivision = random.Next(0, 1) == 1
        };
    }
}