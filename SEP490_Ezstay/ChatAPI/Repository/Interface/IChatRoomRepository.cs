
using ChatAPI.Models;

namespace ChatAPI.Repository.Interface;

public interface IChatRoomRepository
{
  //  IQueryable<ChatRoom> GetAllAsQueryable();
    Task<ChatRoom> GetById(Guid id);
    Task<ChatRoom> Add(ChatRoom chatRoom);
    Task<IEnumerable<ChatRoom>> GetAllChatRoom(Guid accountId);
  Task<ChatRoom?> GetByPostAndUsers(Guid postId, Guid tenantId);
  
   // Task UpdateLastMessageAt(Guid roomId, DateTime sentAt);
}