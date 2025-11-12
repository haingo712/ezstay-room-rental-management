namespace AmenityAPI.Service.Interface;

public interface IRoomAmenityService
{
    Task<bool> RoomAmenityExistsByAmenityId(Guid amenityId);
}