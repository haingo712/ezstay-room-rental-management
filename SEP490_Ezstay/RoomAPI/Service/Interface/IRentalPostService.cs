namespace RoomAPI.Service.Interface;

public interface IRentalPostService
{
    Task<bool> RentalPostExistsByRoomId(Guid roomId);
}