using System;

namespace FFXIVVenues.VenueModels.V2022
{
    public class Notice
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public NoticeType Type { get; set; }
        public string Message { get; set; }
    }

}
