using RoomAPI.Model;

namespace RoomAPI.Repository.Interface;

public interface IRoomAmenityRepository
{
    IQueryable<RoomAmenity> GetAll();
    IQueryable<RoomAmenity> GetAllByRoomId(Guid roomId);
    Task<List<RoomAmenity>> GetListByRoomIdAsync(Guid roomId);
    Task<RoomAmenity> GetById(Guid id);
    Task AddAmenity(IEnumerable<RoomAmenity> roomAmenities);
    Task DeleteAmenity(IEnumerable<Guid> roomAmenityId);
    Task<bool> AmenityIdExistsInRoom(Guid roomId, Guid amenityId);
    Task<bool> CheckAmenity(Guid amenityId);
    
}