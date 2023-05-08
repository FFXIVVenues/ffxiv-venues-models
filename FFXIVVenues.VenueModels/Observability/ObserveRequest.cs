using System.Collections.Generic;

namespace FFXIVVenues.VenueModels.Observability
{
    public record ObserveRequest(
        IEnumerable<ObservableOperation> OperationCriteria, 
        ObservableKey? KeyCriteria,
        string ValueCriteria);
}