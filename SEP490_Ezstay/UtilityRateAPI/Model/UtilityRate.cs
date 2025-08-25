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
  //  [BsonRepresentation(BsonType.String)]
    public UtilityType Type { get; set; }

    public int Tier { get; set; }
    public decimal From { get; set; } 
    public decimal To { get; set; } 
    public decimal Price { get; set; } 
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
