namespace RoomAPI.Service.Interface;

public interface IRentalPostClientService
{
    Task<bool> HasPostsForRoomAsync(Guid roomId);
}