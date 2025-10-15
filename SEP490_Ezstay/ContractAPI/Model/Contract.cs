using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ContractAPI.Enum;
using Shared.Enums;

namespace ContractAPI.Model;
    [BsonIgnoreExtraElements]
    public class Contract
    {
        [BsonId] 
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid OwnerId { get; set; }
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid RoomId {get; set;}
        
        public IdentityProfile SignerProfile {get; set;}
        
        public List<IdentityProfile> ProfilesInContract { get; set; }
        public DateTime CheckinDate { get; set;}
        public DateTime CheckoutDate { get; set;}
        public ContractStatus  ContractStatus{ get; set;}
        public int NumberOfOccupants { get; set; }
        public string Notes { get; set; }
       
        public string Reason { get; set; }
        public decimal RoomPrice { get; set;}
        
        public decimal DepositAmount { get; set; }  //  Tiền cọc
        public DateTime CreatedAt {get;set;}
        public DateTime UpdatedAt {get;set;}
        public List<string> ContractImage { get; set; }
        public DateTime ContractUploadedAt { get; set; }
        
        public DateTime CanceledAt { get; set; }
    }