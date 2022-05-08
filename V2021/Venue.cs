namespace VenueModels.V2021
{

    public class Venue
    {
        public string id { get; set; }
        public string name { get; set; }
        public string[] description { get; set; }
        public string location { get; set; }
        public string discord { get; set; }
        public bool sfw { get; set; }
        public bool nsfw { get; set; }
        public Time[] times { get; set; }
        public string banner { get; set; }
        public string shard { get; set; }
        public string website { get; set; }
        public string[] notices { get; set; }
        public string[] tags { get; set; }
        public string[] comments { get; set; }
        public string[] contacts { get; set; }
        public DateTime added { get; set; }
        public Exception[] exceptions { get; set; }
    }

}
