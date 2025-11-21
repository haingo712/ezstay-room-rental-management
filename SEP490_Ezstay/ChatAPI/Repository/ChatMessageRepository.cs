using ChatAPI.Models;
using ChatAPI.Repository.Interface;
using MongoDB.Driver;

namespace ChatAPI.Repository;

public class ChatMessageRepository:IChatMessageRepository
{
    private readonly IMongoCollection<ChatMessage> _chatMessages;
    
    public ChatMessageRepository(IMongoDatabase database)
    {
        _chatMessages = database.GetCollection<ChatMessage>("ChatMessages");
    }
    public async Task<IEnumerable<ChatMessage>> GetByChatRoomId(Guid chatRoomId)
    {
        return await _chatMessages.Find(m => m.ChatRoomId == chatRoomId)
            .SortBy(m => m.SentAt)
            .ToListAsync();
    }

    // public async Task<ChatMessage?> GetLastMessage(Guid roomId)
    // {
    //     return await _chatMessage.Find(m => m.ChatRoomId == roomId)
    //         .SortByDescending(m => m.SentAt)
    //         .FirstOrDefaultAsync();
    // }

    public async Task<ChatMessage> GetById(Guid id)
    => await _chatMessages.Find(m => m.Id == id).FirstOrDefaultAsync();
    

    public async Task Add(ChatMessage chatMessage)
    {
        await _chatMessages.InsertOneAsync(chatMessage);
    }
    // public async  Task Update(ChatMessage chatMessage)
    // {
    //     await _chatMessage.ReplaceOneAsync(r => r.Id == chatMessage.Id, chatMessage);
    // }

   
    public async Task Delete(ChatMessage chatMessage)
    { 
        await _chatMessages.DeleteOneAsync(r => r.Id == chatMessage.Id);
    }

    public async Task MarkAsRead(Guid roomId, Guid receiverId)
    {
        var filter = Builders<ChatMessage>.Filter.And(
            Builders<ChatMessage>.Filter.Eq(m => m.ChatRoomId, roomId),
            Builders<ChatMessage>.Filter.Ne(m => m.SenderId, receiverId),
            Builders<ChatMessage>.Filter.Eq(m => m.IsRead, false)
        );
        var update = Builders<ChatMessage>.Update.Set(m => m.IsRead, true);
        await _chatMessages.UpdateManyAsync(filter, update);
    }
}