using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatAPI.Models;

public class ChatMessage
{
    
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; } 

    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid ChatRoomId { get; set; }  

    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid SenderId { get; set; }
    public List<string> Image { get; set; }
    public string Content { get; set; } 
    public DateTime SentAt { get; set; } 
    public bool IsRead { get; set; } // Tin nhắn đã đọc chưa
}