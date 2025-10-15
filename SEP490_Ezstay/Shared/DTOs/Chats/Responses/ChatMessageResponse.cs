namespace Shared.DTOs.Chats.Responses;

public class ChatMessageResponse
{
    public Guid Id { get; set; }
    public Guid ChatRoomId { get; set; }
    public Guid SenderId { get; set; }
    public string Image { get; set; }
    public string Content { get; set; }
    public DateTime SentAt { get; set; }
    public bool IsRead { get; set; }
   
}