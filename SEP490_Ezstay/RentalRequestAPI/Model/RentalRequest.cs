using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RentalRequestAPI.Model;

public class RentalRequest
{
    [BsonId] 
    [BsonRepresentation(BsonType.String)] 
    public Guid Id { get; set; } = Guid.NewGuid();
    [BsonRepresentation(BsonType.String)]
    public Guid UserId { get; set; }
    [BsonRepresentation(BsonType.String)]
    public Guid RoomId { get; set; }
    public string Status { get; set; } 
    
    
    
}
