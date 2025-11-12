using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ContractAPI.Model
{
    public class IdentityProfile
    {
        [BsonId]
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; } = Guid.NewGuid();
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid UserId { get; set; } 
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Occupation { get; set; }
        public string Gender { get; set; }
        public string ProvinceId { get; set; }     // Mã tỉnh
        public string ProvinceName { get; set; }   // Tên tỉnh
        public string WardId { get; set; }         // Mã xã/phường
        public string WardName { get; set; }       // Tên xã/phường
        public string Address { get; set; }
        public string TemporaryResidence { get; set; }
        public string CitizenIdNumber { get; set; }
        public DateTime CitizenIdIssuedDate { get; set; }
        public string CitizenIdIssuedPlace { get; set; }
        public string Notes { get; set; }
        public string AvatarUrl { get; set; }
        public string FrontImageUrl { get; set; }
        public string BackImageUrl { get; set; }
        public bool  IsSigner{ get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}