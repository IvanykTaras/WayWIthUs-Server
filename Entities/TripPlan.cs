using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WayWIthUs_Server.Entities
{
    public class TripPlan
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public ICollection<Hotel> Hotels { get; set; }
        public ICollection<itinerary> Itinerary { get; set; }
    }

    public class Hotel
    {
        public string name { get; set; }
        public string address { get; set; }
        public string price { get; set; }
        public string image_url { get; set; }
        public string geo_coordinates { get; set; }
        public string rating { get; set; }
        public string description { get; set; }
    }

    public class itinerary
    {
        public int Day { get; set; }
        public ICollection<Place> Places { get; set; }
    }

    public class Place
    {
        public string time { get; set; }
        public string location { get; set; }
        public string details { get; set; }
        public string image_url { get; set; }
        public string geo_coordinates { get; set; }
        public string ticket_pricing { get; set; }
        public string rating { get; set; }
    }
}
