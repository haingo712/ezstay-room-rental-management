using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatAPI.Models;

public class ChatRoom
{
    [BsonId] 
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid OwnerId { get; set; } 

    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid UserId { get; set; } 
}
