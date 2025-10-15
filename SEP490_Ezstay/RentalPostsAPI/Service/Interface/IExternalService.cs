using RentalPostsAPI.DTO.Request;

namespace RentalPostsAPI.Service.Interface
{
    public interface IExternalService
    {
        Task<RoomDto?> GetRoomByIdAsync(Guid roomId);
        Task<BoardingHouseDTO?> GetBoardingHouseByIdAsync(Guid houseId);
        Task<AccountDto?> GetAccountByIdAsync(Guid Id);
        Task<List<string>?> UploadImagesAsync(List<IFormFile> files);
        Task<List<RoomDto>> GetRoomsByBoardingHouseIdAsync(Guid houseId);
    }
}
