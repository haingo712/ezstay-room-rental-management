using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TenantAPI.Model;

public class IdentityProfile
{
        [BsonId]
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FullName { get; set; }              
        public DateTime DateOfBirth { get; set; }         
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public string Address { get; set; }
        public string TemporaryResidence { get; set; }     // Tạm trú
        public string CitizenIdNumber { get; set; }        // CMND/CCCD
        public DateTime CitizenIdIssuedDate { get; set; }  // Ngày cấp
        public string CitizenIdIssuedPlace { get; set; }   // Nơi cấp
        public string Notes { get; set; }                  
        public string AvatarUrl { get; set; }
        public string FrontImageUrl { get; set; }
        public string BackImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
}