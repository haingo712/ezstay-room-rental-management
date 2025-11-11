using RoomAmenityAPI.Model;

namespace RoomAmenityAPI.Repository.Interface;

public interface IRoomAmenityRepository
{
    IQueryable<RoomAmenity> GetAll();
    Task<RoomAmenity?> GetById(Guid id);
    Task<IEnumerable<RoomAmenity?>> GetRoomAmenitiesByRoomId(Guid roomId);
    Task Add(RoomAmenity amenity);
    Task Update(RoomAmenity amenity);
    Task Delete(RoomAmenity amenity);
    Task<bool> AmenityIdExistsInRoom(Guid roomId, Guid amenityId);
    Task<bool> CheckAmenity(Guid amenityId);


}