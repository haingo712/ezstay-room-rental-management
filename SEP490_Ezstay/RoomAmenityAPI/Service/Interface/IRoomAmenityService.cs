using RoomAmenityAPI.DTO.Request;
using RoomAmenityAPI.DTO.Response;

namespace RoomAmenityAPI.Service.Interface;

public interface IRoomAmenityService
{
    IQueryable<RoomAmenityDto> GetAllByRoomId(Guid roonId);
    IQueryable<RoomAmenityDto> GetAll();
  //  IQueryable<RoomAmenityDto> GetAllByOwnerId(Guid ownerId);
    Task<RoomAmenityDto> GetByIdAsync(Guid id);
    Task<ApiResponse<RoomAmenityDto>> AddAsync(CreateRoomAmenityDto request);
    Task<ApiResponse<bool>> UpdateAsync(Guid id,UpdateRoomAmenityDto request);
    Task DeleteAsync(Guid id);
    
    
}