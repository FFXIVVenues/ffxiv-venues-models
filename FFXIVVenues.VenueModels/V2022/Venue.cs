using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FFXIVVenues.VenueModels.V2022
{
    public class Venue
    {
        public string Id { get; set; }
        public string Name { get; set; } = "An mysterious venue";
        public DateTime Added { get; init; } = DateTime.UtcNow;
        public List<string> Description { get; set; } = new();
        public Location Location { get; set; } = new();
        public Uri Website { get; set; }
        public Uri Discord { get; set; }
        public bool Sfw { get; set; }
        public bool Nsfw { get; set; }
        public List<Opening> Openings { get; set; } = new();
        public List<OpenOverride> OpenOverrides { get; set; } = new();
        public List<Notice> Notices { get; set; } = new();
        public List<string> Managers { get; set; } = new();
        public List<string> Tags { get; set; } = new();
        public bool Open => this.IsOpen();

        public Venue() =>
            Id = GenerateId();

        public Venue(V2021.Venue v1Venue)
        {
            this.Id = v1Venue.id;
            this.Description = v1Venue.description?.ToList();
            this.Discord = string.IsNullOrWhiteSpace(v1Venue.discord) ? null : new Uri(v1Venue.discord);
            this.Name = v1Venue.name;
            this.Nsfw = v1Venue.nsfw;
            this.Sfw = v1Venue.sfw;
            this.Website = string.IsNullOrWhiteSpace(v1Venue.website) ? null : new Uri(v1Venue.website);
            this.Tags = v1Venue.tags?.ToList();
            this.Managers = v1Venue.contacts?.ToList();
            this.Notices = v1Venue.notices?.Select(v => new Notice {
                Start = DateTime.UtcNow,
                Message = v,
                End = DateTime.UtcNow.AddYears(2),
                Type = V2022.NoticeType.Information
            }).ToList();
            this.OpenOverrides = v1Venue.exceptions?.Select(e => new OpenOverride
            {
                Open = false,
                Start = DateTime.Parse(e.start),
                End = DateTime.Parse(e.end)
            }).ToList();

            // Location
            if (v1Venue.location != null)
            {
                var location = v1Venue.location.Split(",");
                var newLocation = new Location();
                newLocation.World = location[0];
                newLocation.DataCenter = "Aether";
                newLocation.District = location[1].Trim();
                newLocation.Ward = PullNumber(location[2]);
                if (location[3].Trim().Contains("Apt"))
                    newLocation.Apartment = PullNumber(location[3]);
                else
                    newLocation.Plot = PullNumber(location[3]);
                if (location.Length == 5)
                    newLocation.Room = PullNumber(location[4]);
                if (newLocation.Plot > 30 || location[2].Trim().EndsWith("Sub"))
                    newLocation.Subdivision = true;

                this.Location = newLocation;
            }

            // Openings
            this.Openings = v1Venue.times?.Select(v => new Opening
            {
                Day = (Day)v.day,
                Location = null,
                Start = new Time
                {
                    Hour = (ushort)(v.start.hour - 4 < 0 ? (24 + v.start.hour - 4) : (v.start.hour - 4)),
                    Minute = (ushort)v.start.minute,
                    NextDay = v.start.nextDay & v.start.hour - 4 >= 0,
                    TimeZone = "Eastern Standard Time"
                },
                End = v.end != null ? 
                    new Time {
                        Hour = (ushort)(v.end.hour - 4 < 0 ? (24 + v.end.hour - 4) : (v.end.hour - 4)),
                        Minute = (ushort)v.end.minute,
                        NextDay = v.end.nextDay & v.end.hour - 4 >= 0,
                        TimeZone = "Eastern Standard Time"
                    } : null
            }).ToList() ?? new List<Opening>();
        }

        public static Venue FromV1Venue(V2021.Venue v1Venue) => new Venue(v1Venue);
        
        public string GenerateId()
        {
            var chars = "BCDFGHJKLMNPQRSTVWXYZbcdfghjklmnpqrstvwxyz0123456789";
            var stringChars = new char[12];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new string(stringChars);
        }

        private static ushort PullNumber(string input)
        {
            return ushort.Parse(new Regex("\\d?\\d").Match(input).Value);
        }

        public Opening GetActiveOpening()
        {
            if (Openings == null || Openings.Count == 0)
                return null;

            foreach (var opening in Openings)
                if (opening.IsNow)
                    return opening;

            return null;
        }

        public bool IsOpen()
        {
            if (this.OpenOverrides != null)
            {
                var @override = this.OpenOverrides.FirstOrDefault(o => o.IsNow);
                if (@override != null) return @override.Open;
            }

            return GetActiveOpening() != null;
        }


    }
}
