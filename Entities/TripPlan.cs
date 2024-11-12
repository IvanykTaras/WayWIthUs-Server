using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WayWIthUs_Server.Entities{
    public class TripPlan
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        
        [BsonRequired]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }
        
        [BsonRequired]
        public string Title { get; set; }
        [BsonRequired]
        public string Description { get; set; }
        [BsonRequired]
        public DateTime StartDate { get; set; }
        [BsonRequired]
        public DateTime EndDate { get; set; }
        [BsonRequired]
        public ICollection<CityPlan> CityPlans { get; set; }
        [BsonRequired]
        public ICollection<string> Languages { get; set; }
        [BsonRequired]
        public Age Age { get; set; }
        [BsonRequired]
        public GenderParticipants GenderParticipants { get; set; }
        [BsonRequired]
        public bool WithChildren { get; set; }
        
        [BsonRequired]
        public int Budget { get; set; }
        
        [BsonRequired]
        public BudgetType BudgetType { get; set; }

        [BsonRequired]
        public GroupType GroupType { get; set; }

        [BsonRequired]
        public string TypeTravel { get; set; }
    }

    public class Age
    {
        public int Min { get; set; }
        public int Max { get; set; }
    }

    public enum GenderParticipants
    {
        Male,
        Female,
        Both,
        Other
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
}

