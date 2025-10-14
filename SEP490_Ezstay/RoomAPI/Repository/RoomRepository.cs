
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using RoomAPI.Model;
using RoomAPI.Repository.Interface;

namespace RoomAPI.Repository;

public class RoomRepository:IRoomRepository
{
    private readonly IMongoCollection<Room> _rooms;
    
    public RoomRepository(IMongoDatabase database)
    {
        _rooms = database.GetCollection<Room>("Rooms");
    }

    public IQueryable<Room>  GetAllQueryable()=> _rooms.AsQueryable();
    public async Task<Room?> GetById(Guid id)
    {
      return await _rooms.Find(a => a.Id == id).FirstOrDefaultAsync();
    }

    public async Task Add(Room room)
    {
        await _rooms.InsertOneAsync(room);
    }
    public async Task<bool> RoomNameExists(string roomName)
    => await _rooms.AsQueryable().AnyAsync(r => r.RoomName.ToLower() == roomName.ToLower());
    
    public async Task<bool> RoomNameExistsInHouse(Guid houseId, string roomName)
    {
        return await _rooms.AsQueryable()
            .AnyAsync(r => r.HouseId == houseId 
                           && r.RoomName.ToLower() == roomName.ToLower()
            );
    }
    // public async Task<bool> RoomNameExistsInHouse(Guid houseId, string roomName ,Guid houseLocationId)
    // {
    //     return await _rooms.AsQueryable()
    //         .AnyAsync(r => r.HouseId == houseId 
    //                        && r.RoomName.ToLower() == roomName.ToLower()
    //                        && r.HouseId != houseLocationId
    //                        );
    // }
    public async Task<bool> RoomNameExistsInHouse(Guid houseId, string roomName, Guid roomId)
    {
        return await _rooms.AsQueryable()
            .AnyAsync(r =>
                r.HouseId == houseId &&
                r.RoomName.ToLower() == roomName.ToLower() &&
                r.Id != roomId); 
    }
    public async  Task Update(Room room)
    {
        await _rooms.ReplaceOneAsync(r => r.Id == room.Id, room);
    }

   
    public async Task Delete(Room room)
    { 
        await _rooms.DeleteOneAsync(r => r.Id == room.Id);
    }
}