using System.Collections.Generic;

namespace FFXIVVenues.VenueModels.Observability;

// todo Add observable key/values to this so decisions can be made without fetching the whole venue
public record VenueObservation(ObservableOperation Operation, 
                               string SubjectId,
                               string SubjectName, 
                               bool Approved,
                               string DataCenter, 
                               string World, 
                               List<string> Managers)
    : Observation(Operation, SubjectId, SubjectName)
{
    public static VenueObservation FromVenue(ObservableOperation op, Venue venue) =>
        new(op, venue.Id, venue.Name, venue.Approved, venue.Location.DataCenter, venue.Location.World, venue.Managers);
}