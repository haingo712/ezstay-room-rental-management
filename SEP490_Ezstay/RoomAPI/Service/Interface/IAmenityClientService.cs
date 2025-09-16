using RoomAPI.DTO.Response;

namespace RoomAPI.Service.Interface;

public interface IAmenityClientService
{
    Task<AmenityDto?> GetAmenityById(Guid amenityId);

}