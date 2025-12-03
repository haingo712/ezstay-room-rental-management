using ChatAPI.Models;
using ChatAPI.Repository.Interface;
using MongoDB.Driver;
using MongoDB.Driver.Linq;


namespace ChatAPI.Repository;

public class ChatRepository:IChatRepository
{
    private readonly IMongoCollection<ChatRoom> _chatRooms;
    private readonly IMongoCollection<ChatMessage> _chatMessages;
    
    public ChatRepository(IMongoDatabase database)
    {
        _chatRooms = database.GetCollection<ChatRoom>("Chats");
        _chatMessages = database.GetCollection<ChatMessage>("ChatMessages");
    }

    public async Task<ChatRoom> CreateChatRoom(ChatRoom chatRoom)
    {
        await _chatRooms.InsertOneAsync(chatRoom);
        return chatRoom;
    }

    public async Task<IEnumerable<ChatRoom>> GetAllChatRoomByUser(Guid userId)
    {
        return await _chatRooms.Find(r =>   r.UserId == userId )
            .ToListAsync();
    }

    public async Task<IEnumerable<ChatRoom>> GetAllChatRoomByOwner(Guid ownerId)
    {
        return await _chatRooms.Find(r => r.OwnerId == ownerId)
            .ToListAsync();
    }

    public async Task<IEnumerable<ChatRoom>> GetAllChatRoom(Guid accountId)
    {
        return await _chatRooms.Find(r => r.OwnerId == accountId ||  r.UserId == accountId )
            .ToListAsync();
    }
    public async Task<ChatRoom> GetByOwnerAndUsers(Guid ownerId, Guid userId)
    {
        return await _chatRooms.Find(r =>
            r.OwnerId == ownerId &&
            r.UserId == userId).FirstOrDefaultAsync();
    }
    public async Task UpdateLastMessage(Guid roomId, DateTime sentAt)
    {
         var update =  Builders<ChatRoom>.Update.Set(r => r.LastSentAt, sentAt);
        await _chatRooms.UpdateOneAsync(r => r.Id == roomId, update);
    }
   
    public async Task<IEnumerable<ChatMessage>> GetByChatRoomId(Guid chatRoomId)
    {
        return await _chatMessages.Find(m => m.ChatRoomId == chatRoomId)
            .SortBy(m => m.SentAt)
            .ToListAsync();
    }

    public async Task<ChatMessage> GetById(Guid id)
        => await _chatMessages.Find(m => m.Id == id).FirstOrDefaultAsync();
    

    public async Task AddMessage(ChatMessage chatMessage)
    {
        await _chatMessages.InsertOneAsync(chatMessage);
    }
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