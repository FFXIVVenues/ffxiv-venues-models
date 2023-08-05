using System;

namespace FFXIVVenues.VenueModels
{
    public class Notice
    {
        public string Id { get; set; } = IdHelper.GenerateId(3);
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset End { get; set; }
        public NoticeType Type { get; set; }
        public string Message { get; set; }
        public bool IsNow => this.IsNowInternal();

        private bool IsNowInternal()
        {
            return DateTimeOffset.UtcNow > Start && DateTimeOffset.UtcNow < End;
        }
    }

}
