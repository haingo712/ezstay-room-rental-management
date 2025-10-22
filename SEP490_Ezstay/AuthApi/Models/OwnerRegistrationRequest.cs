using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

using Shared.Enums;

namespace AuthApi.Models
{
    public class OwnerRegistrationRequest
    {
        [BsonId]
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid AccountId { get; set; }

        public string Reason { get; set; } = null!; // Người dùng giải thích lý do

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public RequestStatusEnum Status { get; set; } = RequestStatusEnum.Pending;


        public string? RejectionReason { get; set; }

        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid? ReviewedByStaffId { get; set; }

        public DateTime? ReviewedAt { get; set; }
    }
}
