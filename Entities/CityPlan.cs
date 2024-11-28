using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WayWIthUs_Server.Entities
{
    public class CityPlan
    {
        [BsonRequired]
        public DateTime StartDate { get; set; }
        [BsonRequired]
        public DateTime EndDate { get; set; } 
        [BsonRequired]
        public string OriginLocation { get; set; }
        [BsonRequired]
        public string Image_url { get; set; }
        [BsonRequired]
        public Transport? Transport { get; set; }
        [BsonRequired]
        public ICollection<Accommodation> Accommodations { get; set; }
        [BsonRequired]
        public ICollection<Place> Places { get; set; }
        public string Description { get; set; }
    }

    public class Accommodation
    {
        public string location_acc { get; set; }
        public string name { get; set; }
        public string? description { get; set; }
        public string image_url { get; set; }
        public string googleMapUrl { get; set; }
    }

    public class Place
    {
/*      public string time { get; set; }*/
        public string location { get; set; }
        public string details { get; set; }
        public string image_url { get; set; }
        public string googleMapUrl { get; set; }
    }

    public enum Transport
    {
        Train,
        Car,
        Bus,
        AirPlain,
        OnFeet,
        Ship,
        Bicycle
    }
}
