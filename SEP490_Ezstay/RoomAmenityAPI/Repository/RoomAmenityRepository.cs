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

    public async Task<RoomAmenity?> GetByIdAsync(Guid id)
    {
        return await _roomAmenities.Find(r => r.Id == id).FirstOrDefaultAsync();
    }
    public async Task<IEnumerable<RoomAmenity?>> GetRoomAmenitiesByRoomIdAsync(Guid roomId)
    {
        return await _roomAmenities.Find(r => r.RoomId == roomId).ToListAsync();
    }

    public async Task AddAsync(RoomAmenity roomAmenity)
    {
        await _roomAmenities.InsertOneAsync(roomAmenity);
    }
    public async  Task<bool> AmenityIdExistsInRoomAsync(Guid roomId, Guid amenityId)
    =>    await _roomAmenities
        .Find(r => r.RoomId == roomId && r.AmenityId == amenityId)
    .AnyAsync();
  
    public async  Task UpdateAsync(RoomAmenity roomAmenity)
    {
        await _roomAmenities.ReplaceOneAsync(r => r.Id == roomAmenity.Id, roomAmenity);
    }
    
    public async Task DeleteAsync(RoomAmenity roomAmenity)
    { 
        await _roomAmenities.DeleteOneAsync(r => r.Id == roomAmenity.Id);
    }
}