using RoomAPI.DTO.Request;
using RoomAPI.Model;
using Shared.Enums;

namespace RoomAPI.Repository.Interface;

public interface IRoomRepository
{
  //  IQueryable<Room>  GetAll();
  
    IQueryable<Room> GetAllByHouseId(Guid houseId);
    IQueryable<Room> GetAllStatusActiveByHouseId(Guid houseId, RoomStatus roomStatus);
    Task<Room> GetById(Guid id);
    Task Add(Room room);
    Task Update(Room room);
    Task Delete(Room room);
    Task<bool> RoomNameExistsInHouse(Guid houseId, string roomName);
//    Task<bool> RoomNameExistsInHouse(Guid houseId, string roomName, Guid roomId);

    IQueryable<RoomAmenity> GetAllRoomAmenityByRoomId(Guid roomId);
    Task AddAmenity(IEnumerable<RoomAmenity> roomAmenities);
    Task<bool> CheckAmenity(Guid amenityId);
    Task DeleteByRoomId(Guid roomId);
}