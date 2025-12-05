using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ContractAPI.Model;
    [BsonIgnoreExtraElements]
    public class RentalRequest
    {
        [BsonId]
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid UserId { get; set; } 
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid OwnerId { get; set; }

        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid RoomId { get; set; }  
        
        public DateTime CheckinDate { get; set; }
        
        public DateTime CheckoutDate { get; set; }
        
        public int NumberOfOccupants { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<string> CitizenIdNumber { get; set;}
    }
    