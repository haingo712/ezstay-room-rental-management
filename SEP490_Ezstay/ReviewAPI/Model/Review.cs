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
    
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid PostId { get; set; }
    
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid ContractId { get; set; }
    
    public int Rating { get; set; } 
    public string Content { get; set; }
    
    // public ReviewStatus Status { get; set; }
    public DateTime ReviewDeadline { get; set; }
    
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid ImageId { get; set; }
    public DateTime CreatedAt { get; set; } 
    public DateTime UpdatedAt { get; set; }
   
    public bool IsDetele { get; set; }

    public DateTime DeletedAt { get; set; }

}
