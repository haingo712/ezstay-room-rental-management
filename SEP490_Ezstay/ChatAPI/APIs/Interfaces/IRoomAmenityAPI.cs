namespace AmenityAPI.APIs.Interfaces;

public interface IRoomAmenityAPI
{
    Task<bool> HasRoomAmenityByAmenityId(Guid amenityId);
}