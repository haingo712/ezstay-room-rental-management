using RoomAPI.DTO.Request;

using Shared.DTOs;
using Shared.DTOs.RoomAmenities.Responses;

namespace RoomAPI.APIs.Interfaces;

public interface IRoomAmenityAPI
{
    Task<ApiResponse<List<RoomAmenityResponse>>> AddRoomAmenitiesAsync(Guid roomId, List<CreateRoomAmenity> request);

}