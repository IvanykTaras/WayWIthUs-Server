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
    public string Username { get; set; }
    
    [BsonRequired]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; }
    
    [BsonRequired]
    public string Password { get; set; } 
}
