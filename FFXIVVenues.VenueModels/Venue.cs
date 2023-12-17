using System;
using System.Collections.Generic;
using System.Linq;

namespace FFXIVVenues.VenueModels;

public class Venue
{
    public string Id { get; set; } = IdHelper.GenerateId();
    public string Name { get; set; } = "A mysterious venue";
    public Uri BannerUri { get; init; }
    public DateTimeOffset Added { get; init; } = DateTimeOffset.UtcNow;
    public List<string> Description { get; set; } = new();
    public Location Location { get; set; } = new();
    public Uri Website { get; set; }
    public Uri Discord { get; set; }
    public bool Hiring { get; set; }
    public bool Sfw { get; set; }
    public List<Schedule> Schedule { get; set; } = new();
    public List<ScheduleOverride> ScheduleOverrides { get; set; } = new();
    public List<Notice> Notices { get; set; } = new();
    public List<string> Managers { get; set; } = new();
    public List<string> Tags { get; set; } = new();
    public bool Approved { get; set; }
    public DateTimeOffset? LastModified { get; set; }
    public string MareCode { get; set; }
    public string MarePassword { get; set; }
    public Opening? Resolution => _resolutionCache??=this.Resolve(DateTimeOffset.UtcNow);
    private Opening? _resolutionCache = null;

    public Opening? Resolve(DateTimeOffset at)
    {
        var overrides = this.ScheduleOverrides.OrderBy(o => o.Start).ToList();
        var openingOverride = overrides.FirstOrDefault(o => o is { Open: true } && o.End > at);
        if (openingOverride is { IsNow: true }) return new (openingOverride.Start, openingOverride.End);

        if (Schedule is null || Schedule.Count == 0)
            return openingOverride is not null ? new(openingOverride.Start, openingOverride.End) : null;
        
        var closingOverrides = overrides.Where(o => o is { Open: false } && o.End > at).ToList();
        
        Opening? earliestOpening = null;
        
        var earliestClosingOverride = closingOverrides.FirstOrDefault();
        if (earliestClosingOverride is not { IsNow: true })
        {
            earliestOpening = this.Schedule
                .Select(s => s.Resolve(at))
                .OrderBy(s => s.Start)
                .FirstOrDefault(s =>
                    earliestClosingOverride is null || s.Start < earliestClosingOverride.Start);
            if (earliestClosingOverride is not null && earliestOpening is not null)
                earliestOpening = earliestOpening.Truncate(earliestClosingOverride.Start, earliestClosingOverride.End).FirstOrDefault();
        }

        if (earliestOpening is null)
            for (var i = 0; i < closingOverrides.Count; i++)
            {
                var closingOverride = closingOverrides[i];
                var nextClosingOverride = i + 1 < closingOverrides.Count ? closingOverrides[i + 1] : null;
                earliestOpening = this.Schedule
                    .Select(s => s.Resolve(closingOverride.End))
                    .Where(rs => nextClosingOverride is null || rs.Start < nextClosingOverride.Start)
                    .MinBy(rs => rs.Start);

                if (nextClosingOverride is not null && earliestOpening is not null)
                    earliestOpening = earliestOpening.Truncate(nextClosingOverride.Start, nextClosingOverride.End).FirstOrDefault();
                
                // If an opening schedule is found, break the loop
                if (earliestOpening is not null)
                    break;
            }
        
        if (openingOverride is not null && openingOverride.Start < earliestOpening!.Start)
            return new(openingOverride.Start, openingOverride.End > earliestOpening.Start ? earliestOpening.End : openingOverride.End);
        
        return earliestOpening;
    }


}