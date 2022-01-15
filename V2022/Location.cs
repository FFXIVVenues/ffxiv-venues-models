namespace VenueModels.V2022
{
    public class Location
    {
        public string DataCenter { get; set; }
        public string World { get; set; }
        public string District { get; set; }
        public ushort Ward { get; set; }
        public ushort Plot { get; set; }
        public ushort Apartment { get; set; }
        public bool Subdivision { get; set; }
        public string Shard { get; set; }
    }
}