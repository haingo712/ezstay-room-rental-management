using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace AuthApi.Models
{
    public class EmailVerification
    {
        [BsonId]
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Email { get; set; } = null!;
        public string OtpCode { get; set; } = null!;
        public DateTime ExpiredAt { get; set; }
        public bool IsVerified { get; set; } = false;
        public string UserPayload { get; set; } = null!; // Store serialized user data
    }
}
