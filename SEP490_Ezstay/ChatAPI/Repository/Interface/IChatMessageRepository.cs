using ChatAPI.Models;

namespace ChatAPI.Repository.Interface;

public interface IChatMessageRepository
{
    Task<IEnumerable<ChatMessage>> GetByChatRoomId(Guid chatRoomId);
    Task<ChatMessage> GetById(Guid id);
    Task Add(ChatMessage message);
    Task Delete(ChatMessage chatMessage);
    Task MarkAsRead(Guid roomId, Guid receiverId);
}