using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace RentalPostsAPI.Models
{
    public class RentalPosts
    {
        [BsonId]
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid AuthorId { get; set; }

        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public List<Guid>? RoomId { get; set; }

        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid BoardingHouseId { get; set; }
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid? ImageId { get; set; }
        public List<string>? ImageUrls { get; set; }
        public string Title { get; set; } = null!;

        public string Content { get; set; } = null!;

        public string ContactPhone { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid? DeletedBy { get; set; }

        public DateTime? DeletedAt { get; set; }

        public int? IsApproved { get; set; }
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid? ApprovedByStaff { get; set; }

        public DateTime? ApprovedAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}

