using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ReviewAPI.Enum;

namespace ReviewAPI.Model;

public class Review
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; } = Guid.NewGuid();
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid UserId { get; set; } 
    public List<string> ImageUrl  { get; set; }
    
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid RoomId { get; set; }
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid OwnerId { get; set; }

    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid ContractId { get; set; }
    public bool IsHidden { get; set; }

    public int Rating { get; set; } 
    public string Content { get; set; }
    public DateTime ReviewDeadline { get; set; }
    public DateTime CreatedAt { get; set; } 
    public DateTime UpdatedAt { get; set; }
   
    public bool IsDetele { get; set; }

    public DateTime DeletedAt { get; set; }

}
