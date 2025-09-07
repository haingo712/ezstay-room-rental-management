using RoomAPI.DTO.Request;
using RoomAPI.DTO.Response;

namespace RoomAPI.Service.Interface;

public interface IRoomService
{
    IQueryable<RoomDto>  GetAllQueryable();
    //Task<IEnumerable<RoomDto>> GetAllByHouseId(int houseId);
    IQueryable<RoomDto> GetAllByHouseId(Guid houseId);
  //  IQueryable<RoomDto> GetAllByHouseLocationId(Guid houseLocationId);
    Task<RoomDto> GetById(Guid id);
   // Task<ApiResponse<RoomDto>> Add(CreateRoomDto request);
    Task<ApiResponse<RoomDto>> Add(Guid houseId, Guid houseLocationId, CreateRoomDto request);
  
    Task<ApiResponse<bool>> Update(Guid id,UpdateRoomDto request);
    Task Delete(Guid id);
}