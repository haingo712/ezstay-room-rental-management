using RoomAmenityAPI.Model;

namespace RoomAmenityAPI.Repository.Interface;

public interface IRoomAmenityRepository
{
    IQueryable<RoomAmenity> GetAll();
    Task<RoomAmenity?> GetByIdAsync(Guid id);
    Task<IEnumerable<RoomAmenity?>> GetRoomAmenitiesByRoomIdAsync(Guid roomId);
    Task AddAsync(RoomAmenity amenity);
    Task UpdateAsync(RoomAmenity amenity);
    Task DeleteAsync(RoomAmenity amenity);
    Task<bool> AmenityIdExistsInRoomAsync(Guid roomId, Guid amenityId);
    Task<bool> CheckAmenity(Guid amenityId);


}