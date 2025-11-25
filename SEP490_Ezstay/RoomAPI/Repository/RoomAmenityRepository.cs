using RoomAPI.Model;
using RoomAPI.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;


namespace RoomAPI.Repository;

public class RoomAmenityRepository:IRoomAmenityRepository
{
    private readonly IMongoCollection<RoomAmenity> _roomAmenities;
    
    public RoomAmenityRepository(IMongoDatabase database)
    {
        _roomAmenities = database.GetCollection<RoomAmenity>("RoomAmenities");
    }
    

    public IQueryable<RoomAmenity> GetAll()=> _roomAmenities.AsQueryable();
    public IQueryable<RoomAmenity> GetAllByRoomId(Guid roomId)
        => _roomAmenities.AsQueryable().Where(a => a.RoomId == roomId);

    public async Task<RoomAmenity> GetById(Guid id)
    {
        return await _roomAmenities.Find(r => r.Id == id).FirstOrDefaultAsync();
    }
    public async Task Add(IEnumerable<RoomAmenity> roomAmenities)
    {
        await _roomAmenities.InsertManyAsync(roomAmenities);
    }

    // public async Task Add(RoomAmenity roomAmenity)
    // {
    //     await _roomAmenities.InsertOneAsync(roomAmenity);
    // }

    public Task<bool> AmenityIdExistsInRoom(Guid roomId, Guid amenityId)
    {
        return _roomAmenities
            .Find(r => r.RoomId == roomId && r.AmenityId == amenityId).AnyAsync();
    }

    public async Task<bool> CheckAmenity(Guid amenityId)
        =>await _roomAmenities.Find(r => r.AmenityId == amenityId).AnyAsync();
    // public async Task DeleteRange(IEnumerable<Guid> roomAmenityIds)
    // {
    //     if (!roomAmenityIds.Any()) return;
    //     await _roomAmenities.DeleteManyAsync(r => roomAmenityIds.Contains(r.Id));
    // }

    public async Task Delete(IEnumerable<Guid> roomAmenityId)
    { 
        await _roomAmenities.DeleteManyAsync(r => roomAmenityId.Contains(r.Id));
    }
}