
using ChatAPI.Models;

namespace ChatAPI.Repository.Interface;

public interface IChatRoomRepository
{
  //  IQueryable<ChatRoom> GetAllAsQueryable();
  //  Task<ChatRoom?> GetById(Guid id);
    Task<ChatRoom> Add(ChatRoom chatRoom);
    Task<IEnumerable<ChatRoom>> GetByChatRoomByOwner(Guid ownerId);
    Task<ChatRoom?> GetByUsersAsync(Guid ownerId, Guid tenantId);
    // Task<ChatRoom?> GetByPostAndUsers(Guid postId, Guid ownerId, Guid tenantId);
  //  Task Delete(ChatRoom chatRoom);
  Task<ChatRoom?> GetByPostAndUsers(Guid postId, Guid tenantId);

  //  Task<ChatRoom?> GetByMembers(Guid postId, Guid ownerId, Guid tenantId);
    Task<IEnumerable<ChatRoom>> GetByOwner(Guid ownerId);
    Task<IEnumerable<ChatRoom>> GetByTenant(Guid tenantId);
   // Task UpdateLastMessageAt(Guid roomId, DateTime sentAt);
}