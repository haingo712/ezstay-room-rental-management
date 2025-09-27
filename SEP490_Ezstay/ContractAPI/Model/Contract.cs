using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ContractAPI.Enum;

namespace ContractAPI.Model
{
    public class Contract
    {
        [BsonId] 
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid OwnerId { get; set; }
        
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid TenantId { get; set; }  // Người thuê (trước đây là UserId)
        
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid RoomId {get; set;}
        
        public DateTime CheckinDate { get; set;}
        public DateTime CheckoutDate { get; set;}
        public ContractStatus  ContractStatus{ get; set;}
        public int NumberOfOccupants { get; set; }
        public string Notes { get; set; }
        public string Reason { get; set; }
        
        public decimal DepositAmount { get; set; }  //  Tiền cọc
        public DateTime CreatedAt {get;set;}
        public DateTime UpdatedAt {get;set;}
        
        public DateTime CanceledAt { get; set; }

    }
}