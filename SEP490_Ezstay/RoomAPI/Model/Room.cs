using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RoomAPI.Model
{
    public class Room
    {
        [BsonId] 
        [BsonRepresentation(BsonType.String)] 
        public Guid Id { get; set; } = Guid.NewGuid();
        [BsonRepresentation(BsonType.String)] 
        public Guid HouseId { get; set; }
        [BsonRepresentation(BsonType.String)] 
        public Guid HouseLocationId { get; set; }
        public string RoomName { get; set; } = null!;
      
        public decimal? Area { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
