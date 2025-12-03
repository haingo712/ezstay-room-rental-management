
using ChatAPI.Models;

namespace ChatAPI.Repository.Interface;

public interface IChatRepository
{
    Task<ChatRoom> CreateChatRoom(ChatRoom chatRoom);
    Task<IEnumerable<ChatRoom>> GetAllChatRoomByUser(Guid userId);
    Task<IEnumerable<ChatRoom>> GetAllChatRoomByOwner(Guid ownerId);
   
    Task<ChatRoom> GetByOwnerAndUsers(Guid ownerId,Guid userId);
    Task UpdateLastMessage(Guid roomId, DateTime sentAt);
    
    Task<IEnumerable<ChatMessage>> GetByChatRoomId(Guid chatRoomId);
    Task<ChatMessage> GetById(Guid id);
    Task AddMessage(ChatMessage message);
    Task Delete(ChatMessage chatMessage);
    Task MarkAsRead(Guid roomId, Guid receiverId);
}