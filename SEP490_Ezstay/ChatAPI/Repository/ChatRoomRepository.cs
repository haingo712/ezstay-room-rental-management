using ChatAPI.Models;
using ChatAPI.Repository.Interface;
using MongoDB.Driver;
using MongoDB.Driver.Linq;


namespace ChatAPI.Repository;

public class ChatRoomRepository:IChatRoomRepository
{
    private readonly IMongoCollection<ChatRoom> _collection;
    
    public ChatRoomRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<ChatRoom>("Chats");
    }
    
    public async Task<ChatRoom?> GetByUsersAsync(Guid ownerId, Guid tenantId)
        => await _collection.Find(r => r.OwnerId == ownerId && r.TenantId == tenantId)
            .FirstOrDefaultAsync();

    public IQueryable<ChatRoom> GetAllAsQueryable()=> _collection.AsQueryable();
    
    public async Task<ChatRoom?> GetById(Guid id)
    {
      return await _collection.Find(a => a.Id == id).FirstOrDefaultAsync();
    }

    public async Task<ChatRoom> Add(ChatRoom chatRoom)
    {
        await _collection.InsertOneAsync(chatRoom);
        return chatRoom;
    }
    public async Task<IEnumerable<ChatRoom>> GetByChatRoomByOwner(Guid ownerId)
    {
        return await _collection.Find(r => r.OwnerId == ownerId ||  r.TenantId == ownerId )
            .ToListAsync();
    }
  
    public async Task<ChatRoom?> GetByPostAndUsers(Guid postId, Guid tenantId)
    {
        return await _collection.Find(r =>
            r.PostId == postId &&
            r.TenantId == tenantId).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<ChatRoom>> GetByOwner(Guid ownerId)
    {
        return await _collection.Find(r => r.OwnerId == ownerId)
            .SortByDescending(r => r.LastMessageAt)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<ChatRoom>> GetByTenant(Guid tenantId)
    {
        return await _collection.Find(r => r.TenantId == tenantId)
            .SortByDescending(r => r.LastMessageAt)
            .ToListAsync();
    }
    
    // public async Task UpdateLastMessageAt(Guid roomId, DateTime sentAt)
    // {
    //     var update = Builders<ChatRoom>.Update.Set(r => r.LastMessageAt, sentAt);
    //     await _collection.UpdateOneAsync(r => r.Id == roomId, update);
    // }
}