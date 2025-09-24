using RoomAmenityAPI.DTO.Request;
using RoomAmenityAPI.DTO.Response;

namespace RoomAmenityAPI.Service.Interface;

public interface IRoomAmenityService
{
    IQueryable<RoomAmenityResponseDto> GetAllByRoomId(Guid roonId);
    IQueryable<RoomAmenityResponseDto> GetAll();
  //  IQueryable<RoomAmenityDto> GetAllByOwnerId(Guid ownerId);
    Task<RoomAmenityResponseDto> GetByIdAsync(Guid id);
    Task<List<RoomAmenityResponseDto>> GetRoomAmenitiesByRoomIdAsync(Guid roomId);
    Task<ApiResponse<List<RoomAmenityResponseDto>>> AddAsync(Guid roomId, List<CreateRoomAmenityDto> request);
    
}