﻿using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RoomAPI.Enum;

namespace RoomAPI.Model
{
    public class Room
    {
        [BsonId] 
        [BsonGuidRepresentation(GuidRepresentation.Standard)] 
        public Guid Id { get; set; } = Guid.NewGuid();
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid HouseId { get; set; }
      //  [BsonGuidRepresentation(GuidRepresentation.Standard)]
      //  public Guid HouseLocationId { get; set; }
      public string RoomName { get; set; } = null!;
      
        public decimal? Area { get; set; }
        public decimal Price { get; set; }
        public RoomStatus RoomStatus { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
