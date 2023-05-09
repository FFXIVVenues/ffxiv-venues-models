using System.Collections.Generic;
using System.Linq;

namespace FFXIVVenues.VenueModels.Observability
{
    public class ObserveRequest
    {
        
        public ObserveRequest(IEnumerable<ObservableOperation> operationCriteria, 
            ObservableKey? keyCriteria, string valueCriteria)
        {
            this.OperationCriteria = operationCriteria;
            this.KeyCriteria = keyCriteria;
            this.ValueCriteria = valueCriteria;
        }

        public IEnumerable<ObservableOperation> OperationCriteria { get; set; }
        public ObservableKey? KeyCriteria { get; set; }
        public string ValueCriteria { get; set; }
        
        public bool Matches(ObservableOperation operation, VenueModels.Venue venue)
        {
            if (venue == null) return false;
            if (!OperationCriteria.Contains(operation)) return false;
            return KeyCriteria switch
            {
                ObservableKey.Id => venue.Id == ValueCriteria,
                ObservableKey.DataCenter => venue.Location.DataCenter == ValueCriteria,
                ObservableKey.World => venue.Location.World == ValueCriteria,
                ObservableKey.Manager => venue.Managers.Contains(ValueCriteria),
                _ => true,
            };
        }

    };
}