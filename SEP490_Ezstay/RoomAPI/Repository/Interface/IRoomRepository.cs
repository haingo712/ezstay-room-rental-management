using RoomAPI.DTO.Request;
using RoomAPI.Model;

namespace RoomAPI.Repository.Interface;

public interface IRoomRepository
{
    IQueryable<Room>  GetAllQueryable();
  //Task<IEnumerable<Room>> GetAllByHouseId(int houseId);
    Task<Room?> GetById(Guid id);
    Task Add(Room request);
    Task Update(Room room);
    Task Delete(Room room);
    Task<bool> RoomNameExists(string roomName);
    // Task<bool> RoomNameExistsInHouse(Guid houseId, string roomName, Guid houseLocationId);
    Task<bool> RoomNameExistsInHouse(Guid houseId, string roomName);
    Task<bool> RoomNameExistsInHouse(Guid houseId, string roomName, Guid roomId);


}