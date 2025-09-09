using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ReviewResponseAPI.Model;

public class ReviewResponse
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; } = Guid.NewGuid();
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid OwnerId { get; set; } 
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid ReviewId { get; set; }
    public string ResponeContent { get; set; }
    public DateTime CreatedAt { get; set; } 
    public DateTime UpdatedAt { get; set; }
}