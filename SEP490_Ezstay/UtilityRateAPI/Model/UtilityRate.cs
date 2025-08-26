using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using UtilityRateAPI.Enum;

namespace UtilityRateAPI.Model;

public class UtilityRate
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; } = Guid.NewGuid();
    [BsonRepresentation(BsonType.String)]
    public Guid OwnerId { get; set; } 
    public UtilityType Type { get; set; }
    public int Tier { get; set; }
    public int From { get; set; } 
    public int To { get; set; } 
    
    public decimal Price { get; set; } 
   // public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } 
    public DateTime UpdatedAt { get; set; }
}
