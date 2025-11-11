using RoomAmenityAPI.DTO.Request;
using Shared.DTOs;
using Shared.DTOs.RoomAmenities.Responses;

namespace RoomAmenityAPI.Service.Interface;

public interface IRoomAmenityService
{
    IQueryable<RoomAmenityResponse> GetAllByRoomId(Guid roonId);
    IQueryable<RoomAmenityResponse> GetAll();
    Task<RoomAmenityResponse> GetByIdAsync(Guid id);
    Task<List<RoomAmenityResponse>> GetRoomAmenitiesByRoomIdAsync(Guid roomId);
    Task<ApiResponse<List<RoomAmenityResponse>>> AddAsync(Guid roomId, List<CreateRoomAmenity> request);
    Task<bool> CheckAmenity(Guid amenityId);
    Task<bool> DeleteAmenityByRoomId(Guid roomId);

}