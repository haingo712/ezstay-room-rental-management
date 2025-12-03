

using Shared.DTOs.Amenities.Responses;

namespace RoomAPI.Service.Interface;

public interface IAmenityService
{
    Task<AmenityResponse> GetAmenityById(Guid amenityId);

}