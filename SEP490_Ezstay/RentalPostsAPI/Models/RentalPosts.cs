using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace RentalPostsAPI.Models
{
    public class RentalPosts
    {

        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid AuthorId { get; set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid RoomId { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public string ContactPhone { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid? DeletedBy { get; set; }

        public DateTime? DeletedAt { get; set; }

        public int? IsApproved { get; set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid? ApprovedBy { get; set; }

        public DateTime? ApprovedAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}

