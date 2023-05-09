using System.Text.Json.Serialization;

namespace FFXIVVenues.VenueModels.Observability
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ObservableKey
    {
        Manager,
        Id,
        DataCenter,
        World,
        Approved
    }
}