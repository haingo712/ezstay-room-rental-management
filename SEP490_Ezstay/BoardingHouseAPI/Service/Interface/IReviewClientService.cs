using BoardingHouseAPI.DTO.Response;

namespace BoardingHouseAPI.Service.Interface
{
    public interface IReviewClientService
    {
        Task<List<ReviewResponse>?> GetReviewsByRoomsAsync(List<RoomResponse> rooms);
    }
}
