using System.Text.Json.Serialization;

namespace FFXIVVenues.VenueModels.Observability
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ObservableOperation
    {
        Create,
        Update,
        Delete
    }
}