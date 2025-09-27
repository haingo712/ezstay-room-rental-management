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
        public Guid ContractId { get; set; }   // 1 Contract có nhiều IdentityProfile
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ProvinceId { get; set; }     // Mã tỉnh
        public string ProvinceName { get; set; }   // Tên tỉnh
        public string WardId { get; set; }         // Mã xã/phường
        public string WardName { get; set; }       // Tên xã/phường
        // public string CommuneId { get; set; }
        // public string CommuneName { get; set; } // xã
        public string Address { get; set; }
        public string TemporaryResidence { get; set; }
        public string CitizenIdNumber { get; set; }
        public DateTime CitizenIdIssuedDate { get; set; }
        public string CitizenIdIssuedPlace { get; set; }
        public string Notes { get; set; }
        public string AvatarUrl { get; set; }
        public string FrontImageUrl { get; set; }
        public string BackImageUrl { get; set; }
      //  public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
       // public DateTime DeletedAt { get; set; }
    }
}