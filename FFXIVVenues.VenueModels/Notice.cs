using System;

namespace FFXIVVenues.VenueModels.V2022
{
    public class Notice
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public NoticeType Type { get; set; }
        public string Message { get; set; }
        public bool IsNow => this.IsNowInternal();

        private bool IsNowInternal()
        {
            return DateTime.UtcNow > Start && DateTime.UtcNow < End;
        }
    }

}
