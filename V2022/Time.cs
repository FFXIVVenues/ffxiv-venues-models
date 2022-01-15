namespace VenueModels.V2022
{
    public class Time
    {
        private short _hour;
        private short _minute;
        private string _timeZone = TimeZoneInfo.Utc.Id;

        public short Hour { get => _hour; set { if (value < 0 || value > 23) throw new ArgumentOutOfRangeException(nameof(Hour), $"Hour must be set to an integer between 0 and 23 (inclusive)."); _hour = value; } }
        public short Minute { get => _minute; set { if (value < 0 || value > 59) throw new ArgumentOutOfRangeException(nameof(Minute), $"Minute must be set to an integer between 0 and 59 (inclusive)."); _minute = value; } }
        public string TimeZone { get => _timeZone; set => _timeZone = value; }
        public bool NextDay { get; set; }
    }

}
