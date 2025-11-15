using RoomAmenityAPI.Model;

namespace RoomAmenityAPI.Repository.Interface;

public interface IRoomAmenityRepository
{
    IQueryable<RoomAmenity> GetAll();
    IQueryable<RoomAmenity> GetAllByRoomId(Guid roomId);
    Task<RoomAmenity> GetById(Guid id);
    Task Add(IEnumerable<RoomAmenity> roomAmenities);
    Task Delete(IEnumerable<Guid> roomAmenityId);
    Task<bool> AmenityIdExistsInRoom(Guid roomId, Guid amenityId);
    Task<bool> CheckAmenity(Guid amenityId);
    
}