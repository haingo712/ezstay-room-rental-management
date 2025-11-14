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
    public async Task<ChatRoom> GetById(Guid id)
    {
      return await _collection.Find(a => a.Id == id).FirstOrDefaultAsync();
    }

    public async Task<ChatRoom> Add(ChatRoom chatRoom)
    {
        await _collection.InsertOneAsync(chatRoom);
        return chatRoom;
    }
    
    public async Task<IEnumerable<ChatRoom>> GetAllChatRoom(Guid accountId)
    {
        return await _collection.Find(r => r.OwnerId == accountId ||  r.UserId == accountId )
            .ToListAsync();
    }
    public async Task<ChatRoom?> GetByOwnerAndUsers(Guid ownerId, Guid userId)
    {
        return await _collection.Find(r =>
            r.OwnerId == ownerId &&
            r.UserId == userId).FirstOrDefaultAsync();
    }
    
    // public async Task<IEnumerable<ChatRoom>> GetByTenant(Guid userId)
    // {
    //     return await _collection.Find(r => r.UserId == userId)
    //         .SortByDescending(r => r.CreatedAt)
    //         .ToListAsync();
    // }
    
    // public async Task UpdateLastMessageAt(Guid roomId, DateTime sentAt)
    // {
    //     var update = Builders<ChatRoom>.Update.Set(r => r.LastMessageAt, sentAt);
    //     await _collection.UpdateOneAsync(r => r.Id == roomId, update);
    // }
}