using System.Text;
using System.Text.RegularExpressions;

namespace VenueModels.V2022
{
    public class Venue
    {
        public string Id { get; set; }
        public string Name { get; set; } = "An mysterious venue";
        public List<string> Description { get; set; } = new();
        public Location Location { get; set; } = new();
        public Uri Website { get; set; }
        public Uri Discord { get; set; }
        public bool Sfw { get; set; }
        public bool Nsfw { get; set; }
        public List<Opening> Openings { get; set; } = new();
        public List<OpeningException> Exceptions { get; set; } = new();
        public List<Notice> Notices { get; set; } = new();
        public List<string> Contacts { get; set; } = new();
        public List<string> Tags { get; set; } = new();
        public bool Open { get; set; }

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
            this.Tags = v1Venue.tags.ToList();
            this.Contacts = v1Venue.contacts.ToList();
            this.Notices = v1Venue.notices?.Select(v => new Notice {
                Start = DateTime.UtcNow,
                Message = v,
                End = DateTime.UtcNow.AddYears(2),
                Type = V2022.NoticeType.Information
            }).ToList();
            this.Exceptions = v1Venue.exceptions?.Select(e => new OpeningException
            {
                Start = DateTime.Parse(e.start),
                End = DateTime.Parse(e.end)
            }).ToList();

            // Location
            var location = v1Venue.location.Split(",");
            var newLocation = new Location();
            newLocation.World = location[0];
            newLocation.DataCenter = "Aether";
            newLocation.District = location[1].Trim();
            newLocation.Ward = PullNumber(location[2]);
            if (location[3].Trim().Contains("Apartment"))
                newLocation.Apartment = PullNumber(location[3]);
            else
                newLocation.Plot = PullNumber(location[3]);
            if (newLocation.Plot > 30 || location[3].Trim().StartsWith("Sub"))
                newLocation.Subdivision = true;

            this.Location = newLocation;

            // Openings
            this.Openings = v1Venue.times?.Select(v => new Opening
            {
                Day = (Day)v.day,
                Location = null,
                Start = new Time
                {
                    Hour = (ushort)(v.start.hour - 5 < 0 ? (24 + v.start.hour - 5) : (v.start.hour - 5)),
                    Minute = (ushort)v.start.minute,
                    NextDay = v.start.nextDay,
                    TimeZone = "Eastern Standard Time"
                }
            }).ToList() ?? new List<Opening>();
        }

        static Venue FromV1Venue(V2021.Venue v1Venue) => new Venue(v1Venue);
        
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

        public override string ToString()
        {
            var summary = new StringBuilder();

            summary.Append(":id: : ")
                .AppendLine(this.Id)
                .Append(":placard:  : ")
                .AppendLine(this.Name)
                .Append($":hut:  : ")
                .Append(this.Location.DataCenter).Append(", ").Append(this.Location.World).Append(", ")
                .Append(this.Location.District).Append(", Ward ").Append(this.Location.Ward).Append(", ");

            if (this.Location.Apartment > 0)
            {
                summary.Append("Apartment #").Append(this.Location.Apartment);
                if (this.Location.Subdivision) summary.Append(" (subdivision)");
            }
            else
                summary.Append("Plot #").Append(this.Location.Plot);

            summary.AppendLine().Append(":hugging:  : ").AppendLine(this.Sfw ? "Is SFW" : "Is not SFW")
                                .Append(":underage:  : ").AppendLine(this.Nsfw ? "Provides NSFW services" : "Does not provide NSFW services")
                                .Append(":link:  : ").AppendLine(this.Website?.ToString() ?? "No website")
                                .Append(":speaking_head:  : ").AppendLine(this.Discord?.ToString() ?? "No discord");

            if (this.Open)
                summary.AppendLine("🟢 : *Currently open.*");
            else
                summary.AppendLine("🔴 : *Not currently open.*");

            if (this.Openings == null || this.Openings.Count == 0)
                summary.Append(":calendar: : ").AppendLine("No set schedule");
            else
            {
                summary.AppendLine(":calendar: : ");
                foreach (var opening in this.Openings)
                    summary.Append(opening.Day.ToString())
                           .Append(", ")
                           .Append(opening.Start.Hour)
                           .Append(":")
                           .Append(opening.Start.Minute.ToString("00"))
                           .Append(" (")
                           .Append(opening.Start.TimeZone)
                           .Append(") - ")
                           .Append(opening.End.Hour)
                           .Append(":")
                           .Append(opening.End.Minute.ToString("00"))
                           .Append(" (")
                           .Append(opening.End.TimeZone)
                           .AppendLine(")");

            }

            var charsLeft = 1000;
            summary.Append("📝 : ");
            foreach (var paragraph in this.Description)
            {
                if (charsLeft < 10)
                {
                    summary.AppendLine("...");
                    break;
                }

                var trimmmedParagraph = paragraph;
                if (paragraph.Length > charsLeft)
                {
                    trimmmedParagraph = paragraph.Substring(0, charsLeft);
                }
                summary.AppendLine(paragraph).AppendLine();
                charsLeft -= trimmmedParagraph.Length;
            }

            return summary.ToString();
        }

        private static ushort PullNumber(string input)
        {
            return ushort.Parse(new Regex("\\d?\\d").Match(input).Value);
        }

    }
}
