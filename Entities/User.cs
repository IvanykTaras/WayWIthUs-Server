using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WayWIthUs_Server.Entities;

using System.ComponentModel.DataAnnotations;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    [BsonRequired]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string email { get; set; }
    
    [BsonRequired]
    public string family_name { get; set; }
    [BsonRequired]
    public string given_name { get; set; }
    [BsonRequired]
    public string name { get; set; }
    [BsonRequired]
    public string picture { get; set; }
    [BsonRequired]
    public bool verified_email { get; set; }
    [BsonRequired]
    public string? password { get; set; } = null;
}
