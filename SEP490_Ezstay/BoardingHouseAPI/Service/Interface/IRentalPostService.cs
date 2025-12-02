namespace BoardingHouseAPI.Service.Interface;

public interface IRentalPostService
{
    Task<bool> RentalPostExistsByRoomId(Guid roomId);
}