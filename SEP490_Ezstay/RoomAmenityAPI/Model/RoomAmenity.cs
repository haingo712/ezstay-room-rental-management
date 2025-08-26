using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RoomAmenityAPI.Model
{
    public class RoomAmenity
    {
     [BsonId]
     [BsonRepresentation(BsonType.String)] 
        public Guid Id { get; set; }
        [BsonRepresentation(BsonType.String)] 
        public Guid RoomId { get; set; }
        [BsonRepresentation(BsonType.String)] 
        public Guid AmenityId { get; set; }
        
    }
}
