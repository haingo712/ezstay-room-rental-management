using RoomAmenityAPI.DTO.Request;
using RoomAmenityAPI.DTO.Response;

namespace RoomAmenityAPI.Service.Interface;

public interface IRoomAmenityService
{
    IQueryable<RoomAmenityDto> GetAllByRoomId(Guid roonId);
    IQueryable<RoomAmenityDto> GetAll();
  //  IQueryable<RoomAmenityDto> GetAllByOwnerId(Guid ownerId);
    Task<RoomAmenityDto> GetByIdAsync(Guid id);
    Task<List<RoomAmenityDto>> GetRoomAmenitiesByRoomIdAsync(Guid roomId);
    Task<ApiResponse<List<RoomAmenityDto>>> AddAsync(Guid roomId, List<CreateRoomAmenityDto> request);
    Task<ApiResponse<bool>> UpdateAsync(Guid id, UpdateRoomAmenityDto request);
    Task DeleteAsync(Guid id);
    
}