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
        public string DestiantionLocation { get; set; }

        [BsonRequired]
        public ImageUrl Image_url { get; set; }
        [BsonRequired]
        public Transport Transport { get; set; }

        [BsonRequired]
        public ICollection<Hotel> Hotels { get; set; }
        [BsonRequired]
        public ICollection<itinerary> Itinerary { get; set; }
    }


    public enum Transport{
        Train,
        Car,
        Bus,
        AirPlain,
        OnFeet,
        Ship,
        Bicycle
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
        public string googleMapUrl { get; set; }
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
        public string googleMapUrl { get; set; }
    }

    public class ImageUrl{
        public string OriginUrl { get; set; }
        public string DestinationUrl { get; set; }
    }
}
