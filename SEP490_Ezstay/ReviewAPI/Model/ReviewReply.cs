using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ReviewAPI.Model;

public class ReviewReply
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid ReviewId { get; set; }

    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid OwnerId { get; set; }
    
    public List<string>  Image { get; set; }

    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; } 
}