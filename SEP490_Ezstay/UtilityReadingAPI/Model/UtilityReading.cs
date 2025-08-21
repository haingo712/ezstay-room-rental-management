using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UtilityReadingAPI.Model;

public class UtilityReading
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; } = Guid.NewGuid();
    [BsonRepresentation(BsonType.String)]
    public Guid RoomId { get; set; }
    
    public string Type { get; set; }
    
    public DateTime ReadingDate  { get; set;}= DateTime.UtcNow;

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal PreviousIndex { get; set; } 
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal CurrentIndex { get; set;}
   
    public string Note { get; set; }
    
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Consumption => CurrentIndex - PreviousIndex;
  
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Total { get; set;} 
}