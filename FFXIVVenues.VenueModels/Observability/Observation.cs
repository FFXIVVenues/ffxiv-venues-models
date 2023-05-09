namespace FFXIVVenues.VenueModels.Observability
{
    // todo Add observable key/values to this so decisions can be made without fetching the whole venue
    public record Observation(ObservableOperation Operation, string SubjectId, string SubjectName);
}