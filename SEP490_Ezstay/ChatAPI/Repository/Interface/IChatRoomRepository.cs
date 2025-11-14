
using ChatAPI.Models;

namespace ChatAPI.Repository.Interface;

public interface IChatRoomRepository
{
    Task<ChatRoom> GetById(Guid id);
    Task<ChatRoom> Add(ChatRoom chatRoom);
    Task<IEnumerable<ChatRoom>> GetAllChatRoom(Guid accountId);
    Task<ChatRoom?> GetByOwnerAndUsers(Guid ownerId,Guid userId);
   // Task UpdateLastMessageAt(Guid roomId, DateTime sentAt);
}