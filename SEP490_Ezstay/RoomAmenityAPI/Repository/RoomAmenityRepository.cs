using RoomAmenityAPI.Model;
using RoomAmenityAPI.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;


namespace RoomAmenityAPI.Repository;

public class RoomAmenityRepository:IRoomAmenityRepository
{
    private readonly IMongoCollection<RoomAmenity> _roomAmenities;
    
    public RoomAmenityRepository(IMongoDatabase database)
    {
        _roomAmenities = database.GetCollection<RoomAmenity>("RoomAmenities");
    }
    

    public IQueryable<RoomAmenity> GetAll()=> _roomAmenities.AsQueryable();

    public async Task<RoomAmenity?> GetById(Guid id)
    {
        return await _roomAmenities.Find(r => r.Id == id).FirstOrDefaultAsync();
    }
    public async Task<IEnumerable<RoomAmenity?>> GetRoomAmenitiesByRoomId(Guid roomId)
    {
        return await _roomAmenities.Find(r => r.RoomId == roomId).ToListAsync();
    }

    public async Task Add(RoomAmenity roomAmenity)
    {
        await _roomAmenities.InsertOneAsync(roomAmenity);
    }

    public Task<bool> AmenityIdExistsInRoom(Guid roomId, Guid amenityId)
    {
        return _roomAmenities
            .Find(r => r.RoomId == roomId && r.AmenityId == amenityId).AnyAsync();
    }

    public async Task<bool> CheckAmenity(Guid amenityId)
    =>await _roomAmenities.Find(r => r.AmenityId == amenityId).AnyAsync();

    public async  Task<bool> AmenityIdExistsInRoomAsync(Guid roomId, Guid amenityId)
    =>    await _roomAmenities
        .Find(r => r.RoomId == roomId && r.AmenityId == amenityId).AnyAsync();
  
    public async  Task Update(RoomAmenity roomAmenity)
    {
        await _roomAmenities.ReplaceOneAsync(r => r.Id == roomAmenity.Id, roomAmenity);
    }
    
    public async Task Delete(RoomAmenity roomAmenity)
    { 
        await _roomAmenities.DeleteOneAsync(r => r.Id == roomAmenity.Id);
    }
}