using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatAPI.Models;

public class ChatMessage
{
    
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; } 

    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid ChatRoomId { get; set; }  // của phòng chat

    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid SenderId { get; set; } // Ai gửi tin (chủ trọ hoặc người thuê)

    public string Content { get; set; } 
    public DateTime SentAt { get; set; } //  time gửi
    public bool IsRead { get; set; } // Tin nhắn đã đọc chưa
}