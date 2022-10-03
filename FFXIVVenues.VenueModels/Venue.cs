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
        public bool Approved { get; set; }
        public string MareCode { get; set; }
        public bool Open => this.IsOpen();


        public Venue() =>
            Id = GenerateId();
        
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
