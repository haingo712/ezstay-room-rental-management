using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ElectricityReadingAPI.Model;

public class ElectricityReading
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    [BsonRepresentation(BsonType.String)]
    public Guid RoomId { get; set; }
    
    public DateTime CreatedAt { get; set;}= DateTime.UtcNow;
    
    public DateTime ReadingDate  { get; set;}
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal OldIndex { get; set;}
    
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal NewIndex { get; set;}
   
    public string Note { get; set; }
    
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Consumption { get; set;}
  
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Total { get; set;}
    
}