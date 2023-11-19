using System;

namespace FFXIVVenues.VenueModels
{
    public enum Day
    {
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday
    }
    
    public static class DayExtensions
    {
        public static Day Next(this Day day, int numberOfDays = 1) =>
            Scrub(day, numberOfDays);
        
        public static Day Previous(this Day day, int numberOfDays = 1) =>
            Scrub(day, 7-numberOfDays);

        public static Day Scrub(this Day day, int scrub = 1) =>
            (Day)(((int)day + scrub) % 7);

        public static DayOfWeek ToDayOfWeek(this Day day) =>
            (DayOfWeek) ((int)(day + 1) % 7);

    }
}
