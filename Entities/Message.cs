using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using WayWIthUs_Server.Hubs;

namespace WayWIthUs_Server.Entities
{
    public class Message
    {
        [BsonRequired]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonRequired]
        public UserConnection userConnection { get; set; }

        [BsonRequired]
        public string MessageText { get; set; }
    }
}
