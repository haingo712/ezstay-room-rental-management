
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using RoomAPI.Model;
using RoomAPI.Repository.Interface;
using Shared.Enums;

namespace RoomAPI.Repository;

public class RoomRepository:IRoomRepository
{
    private readonly IMongoCollection<Room> _rooms;
    private readonly IMongoCollection<RoomAmenity> _roomAmenities;
    
    public RoomRepository(IMongoDatabase database)
    {
        _rooms = database.GetCollection<Room>("Rooms");
        _roomAmenities = database.GetCollection<RoomAmenity>("RoomAmenities");

    }

    //public IQueryable<Room>  GetAll()=> _rooms.AsQueryable();

    public IQueryable<Room> GetAllByHouseId(Guid houseId)
        => _rooms.AsQueryable().Where(r => r.HouseId == houseId);

    public IQueryable<Room> GetAllStatusActiveByHouseId(Guid houseId, RoomStatus roomStatus)
        => _rooms.AsQueryable().Where(r => r.HouseId == houseId && r.RoomStatus == roomStatus);

    public async Task<Room> GetById(Guid id)
    {
      return await _rooms.Find(a => a.Id == id).FirstOrDefaultAsync();
    }

    public async Task Add(Room room)
    {
        await _rooms.InsertOneAsync(room);
    }
    // public async Task<bool> RoomNameExistsInHouse(Guid houseId, string roomName, Guid roomId)
    // {
    //     return await _rooms.AsQueryable()
    //         .AnyAsync(r =>
    //             r.HouseId == houseId &&
    //             r.RoomName.ToLower() == roomName.ToLower() &&
    //             r.Id != roomId); 
    // }
    public async  Task Update(Room room)
    {
        await _rooms.ReplaceOneAsync(r => r.Id == room.Id, room);
    }

   
    public async Task Delete(Room room)
    { 
        await _rooms.DeleteOneAsync(r => r.Id == room.Id);
    }
    public async Task<bool> RoomNameExistsInHouse(Guid houseId, string roomName)
    {
        return await _rooms.AsQueryable()
            .AnyAsync(r => r.HouseId == houseId 
                           && r.RoomName.ToLower() == roomName.ToLower()
            );
    }
    
    public IQueryable<RoomAmenity> GetAllRoomAmenityByRoomId(Guid roomId)
        => _roomAmenities.AsQueryable().Where(a => a.RoomId == roomId);
    
    public async Task AddAmenity(IEnumerable<RoomAmenity> roomAmenities)
    {
        await _roomAmenities.InsertManyAsync(roomAmenities);
    }
    
    public async Task<bool> CheckAmenity(Guid amenityId)
        =>await _roomAmenities.Find(r => r.AmenityId == amenityId).AnyAsync();

   
    public async Task DeleteByRoomId(Guid roomId)
    {
        await _roomAmenities.DeleteManyAsync(r => r.RoomId == roomId);
    }
}