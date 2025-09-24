using AccountAPI.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AccountAPI.Data
{
    public class User
    {
        [BsonId]
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid UserId { get; set; }

        public GenderEnum Gender { get; set; }

        public string? Avatar { get; set; }
        public string? Bio { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public string? FullName { get; set; }
        public string? Phone { get; set; }

        public string? ProvinceCode { get; set; }
        public string? ProvinceName { get; set; }
        public string? CommuneCode { get; set; }
        public string? CommuneName { get; set; }
    }
}
