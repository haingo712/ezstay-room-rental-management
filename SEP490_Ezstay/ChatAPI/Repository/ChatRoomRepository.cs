using ChatAPI.Models;
using ChatAPI.Repository.Interface;
using MongoDB.Driver;
using MongoDB.Driver.Linq;


namespace ChatAPI.Repository;

public class ChatRoomRepository:IChatRoomRepository
{
    private readonly IMongoCollection<ChatRoom> _chatRooms;
    
    public ChatRoomRepository(IMongoDatabase database)
    {
        _chatRooms = database.GetCollection<ChatRoom>("Chats");
    }
    // public async Task<ChatRoom> GetById(Guid id)
    // {
    //   return await _collection.Find(a => a.Id == id).FirstOrDefaultAsync();
    // }

    public async Task<ChatRoom> Add(ChatRoom chatRoom)
    {
        await _chatRooms.InsertOneAsync(chatRoom);
        return chatRoom;
    }
    
    public async Task<IEnumerable<ChatRoom>> GetAllChatRoom(Guid accountId)
    {
        return await _chatRooms.Find(r => r.OwnerId == accountId ||  r.UserId == accountId )
            .ToListAsync();
    }
    public async Task<ChatRoom?> GetByOwnerAndUsers(Guid ownerId, Guid userId)
    {
        return await _chatRooms.Find(r =>
            r.OwnerId == ownerId &&
            r.UserId == userId).FirstOrDefaultAsync();
    }
    public async Task UpdateLastMessageAt(Guid roomId, DateTime sentAt)
    {
         var update =  Builders<ChatRoom>.Update.Set(r => r.LastSentAt, sentAt);
        await _chatRooms.UpdateOneAsync(r => r.Id == roomId, update);
    }
}