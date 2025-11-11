namespace AmenityAPI.APIs.Interfaces;

public interface IRoomAmenityAPI
{
    Task<bool> RoomAmenityExistsByAmenityId(Guid amenityId);
}