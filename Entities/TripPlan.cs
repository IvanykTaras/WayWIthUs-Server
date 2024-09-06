using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WayWIthUs_Server.Entities
{
    public class TripPlan
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonRequired]
        public string UserEmail { get; set; }
        [BsonRequired]
        public string Location { get; set; }
        [BsonRequired]
        public int DaysNumber { get; set; }
        [BsonRequired]
        public BudgetType BudgetType { get; set; }
        [BsonRequired]
        public GroupType GroupType { get; set; }
        [BsonRequired]
        public ICollection<Hotel> Hotels { get; set; }
        [BsonRequired]
        public ICollection<itinerary> Itinerary { get; set; }
    }

    public enum BudgetType
    {
        Cheap,
        Moderate,
        Luxury
    }

    public enum GroupType
    {
        OnePerson,
        Couple,
        Family,
        Friends
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
