using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AmenityAPI.Models;

public class Amenity
{
    [BsonId] 
    [BsonRepresentation(BsonType.String)] 
    public Guid Id { get; set; } = Guid.NewGuid();
    [BsonRepresentation(BsonType.String)]
    public Guid StaffId { get; set; }
    public string AmenityName { get; set; } = null!;
}
