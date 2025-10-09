

using Shared.DTOs.Amenities.Responses;

namespace RoomAPI.Service.Interface;

public interface IAmenityClientService
{
    Task<AmenityResponse?> GetAmenityById(Guid amenityId);

}