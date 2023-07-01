using System;
using System.Collections.Generic;
using System.Linq;

namespace FFXIVVenues.VenueModels;

public class Venue
{
    public string Id { get; set; } = IdHelper.GenerateId();
    public string Name { get; set; } = "An mysterious venue";
    public Uri BannerUri { get; init; }
    public DateTime Added { get; init; } = DateTime.UtcNow;
    public List<string> Description { get; set; } = new();
    public Location Location { get; set; } = new();
    public Uri Website { get; set; }
    public Uri Discord { get; set; }
    public bool Hiring { get; set; }
    public bool Sfw { get; set; }
    public List<Opening> Openings { get; set; } = new();
    public List<OpenOverride> OpenOverrides { get; set; } = new();
    public List<Notice> Notices { get; set; } = new();
    public List<string> Managers { get; set; } = new();
    public List<string> Tags { get; set; } = new();
    public bool Approved { get; set; }
    public DateTime? LastModified { get; set; }
    public string MareCode { get; set; }
    public string MarePassword { get; set; }
    public bool Open => this.IsOpen();

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