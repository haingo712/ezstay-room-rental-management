using ChatAPI.Models;
using ChatAPI.Repository.Interface;
using MongoDB.Driver;

namespace ChatAPI.Repository;

public class ChatMessageRepository:IChatMessageRepository
{
    private readonly IMongoCollection<ChatMessage> _collection;
    
    public ChatMessageRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<ChatMessage>("ChatMessages");
    }
    public async Task<IEnumerable<ChatMessage>> GetByChatRoomId(Guid chatRoomId)
    {
        return await _collection.Find(m => m.ChatRoomId == chatRoomId)
            .SortBy(m => m.SentAt)
            .ToListAsync();
    }

    public async Task<ChatMessage?> GetLastMessage(Guid roomId)
    {
        return await _collection.Find(m => m.ChatRoomId == roomId)
            .SortByDescending(m => m.SentAt)
            .FirstOrDefaultAsync();
    }

    public async Task Add(ChatMessage message)
    {
        await _collection.InsertOneAsync(message);
    }

    public async Task MarkAsRead(Guid roomId, Guid receiverId)
    {
        var filter = Builders<ChatMessage>.Filter.And(
            Builders<ChatMessage>.Filter.Eq(m => m.ChatRoomId, roomId),
            Builders<ChatMessage>.Filter.Ne(m => m.SenderId, receiverId),
            Builders<ChatMessage>.Filter.Eq(m => m.IsRead, false)
        );
        var update = Builders<ChatMessage>.Update.Set(m => m.IsRead, true);
        await _collection.UpdateManyAsync(filter, update);
    }
}