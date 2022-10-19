using System;
using System.Text;

namespace FFXIVVenues.VenueModels
{
    public class Location
    {
        public string DataCenter { get; set; }
        public string World { get; set; }
        public string District { get; set; }
        public ushort Ward { get; set; }
        public ushort Plot { get; set; }
        public ushort Apartment { get; set; }
        public ushort Room { get; set; }
        public bool Subdivision { get; set; }
        public string Shard { get; set; }
        public string Override { get; set; }

        public override string ToString()
        {
            if (!string.IsNullOrWhiteSpace(this.Override))
                return this.Override;

            var stringBuilder = new StringBuilder();
            stringBuilder.Append(this.DataCenter);
            stringBuilder.Append(", ");
            stringBuilder.Append(this.World);
            stringBuilder.Append(", ");
            stringBuilder.Append(this.District);
            stringBuilder.Append(", Ward ");
            stringBuilder.Append(this.Ward);
            if (this.Apartment != 0 && this.Subdivision)
                stringBuilder.Append(" Sub");
            if (this.Plot != 0)
            {
                stringBuilder.Append(", Plot ");
                stringBuilder.Append(this.Plot);
                if (this.Room != 0)
                {
                    stringBuilder.Append(", Room ");
                    stringBuilder.Append(this.Room);
                }
            }
            else
            {
                stringBuilder.Append(", Apt ");
                stringBuilder.Append(this.Apartment);
            }

            return stringBuilder.ToString();
        }
    }
}