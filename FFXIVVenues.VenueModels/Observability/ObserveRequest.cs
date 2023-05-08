using System.Collections.Generic;

namespace FFXIVVenues.VenueModels.Observability
{
    public class ObserveRequest
    {
        public ObservableKey? KeyCriteria { get; set; }

        public string ValueCriteria { get; set; }

        public IEnumerable<ObservableOperation> OperationCriteria { get; set; }
    }
}