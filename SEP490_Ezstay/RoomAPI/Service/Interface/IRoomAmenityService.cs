
using RoomAPI.DTO.Request;
using Shared.DTOs;
using Shared.DTOs.RoomAmenities.Responses;

namespace RoomAPI.Service.Interface;

public interface IRoomAmenityService
{
    IQueryable<RoomAmenityResponse> GetAllByRoomId(Guid roonId);
    IQueryable<RoomAmenityResponse> GetAll();
    Task<RoomAmenityResponse> GetById(Guid id);
   // Task<List<RoomAmenityResponse>> GetRoomAmenitiesByRoomIdAsync(Guid roomId);
    Task<ApiResponse<List<RoomAmenityResponse>>> UpdateRoomAmenities(Guid roomId, List<CreateRoomAmenity> request);
    Task<bool> CheckAmenity(Guid amenityId);
  //  Task<bool> DeleteAmenityByRoomId(Guid roomId);
}