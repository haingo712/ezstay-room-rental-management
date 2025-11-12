namespace RoomAPI.Service.Interface;

public interface IRentalPostClientService
{
    Task<bool> RentalPostExistsByRoomId(Guid roomId);
}