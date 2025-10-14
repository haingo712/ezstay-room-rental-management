using ChatAPI.Models;

namespace ChatAPI.Repository.Interface;

public interface IChatMessageRepository
{
    Task<IEnumerable<ChatMessage>> GetByChatRoomId(Guid chatRoomId);
    Task<ChatMessage?> GetLastMessage(Guid roomId);
    Task Add(ChatMessage message);
    Task MarkAsRead(Guid roomId, Guid receiverId);
}