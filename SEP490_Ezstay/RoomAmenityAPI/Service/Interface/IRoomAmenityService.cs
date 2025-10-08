using RoomAmenityAPI.DTO.Request;
using RoomAmenityAPI.DTO.Response;

namespace RoomAmenityAPI.Service.Interface;

public interface IRoomAmenityService
{
    IQueryable<RoomAmenityResponse> GetAllByRoomId(Guid roonId);
    IQueryable<RoomAmenityResponse> GetAll();
  //  IQueryable<RoomAmenityDto> GetAllByOwnerId(Guid ownerId);
    Task<RoomAmenityResponse> GetByIdAsync(Guid id);
    Task<List<RoomAmenityResponse>> GetRoomAmenitiesByRoomIdAsync(Guid roomId);
    Task<ApiResponse<List<RoomAmenityResponse>>> AddAsync(Guid roomId, List<CreateRoomAmenity> request);
    Task<bool> CheckAmenity(Guid amenityId);

}