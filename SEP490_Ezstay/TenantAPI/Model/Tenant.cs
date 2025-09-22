using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using TenantAPI.Enum;

namespace TenantAPI.Model
{
    public class Tenant
    {
        [BsonId] 
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; } = Guid.NewGuid();
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid OwnerId { get; set; }
        
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid UserId { get; set; }
        
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid RoomId {get; set;}
        
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid IdentityProfileId {get; set;}
        public DateTime CheckinDate { get; set;}
        public DateTime CheckoutDate { get; set;}
        
        public TenantStatus  TenantStatus{ get; set;}
        public int NumberOfOccupants { get; set; }
        public string? Notes { get; set; }
        public string reason { get; set; }
        
        public decimal DepositAmount { get; set; }  //  Tiền cọc
        public DateTime CreatedAt {get;set;}
        public DateTime UpdatedAt {get;set;}
    }
}

