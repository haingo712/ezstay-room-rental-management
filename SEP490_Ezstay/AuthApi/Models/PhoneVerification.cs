using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace AuthApi.Models
{
    public class PhoneVerification
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        public string Phone { get; set; } = null!;  // Số điện thoại
        public string Otp { get; set; } = null!;    // Mã OTP
        public DateTime ExpireAt { get; set; }
    }
}
