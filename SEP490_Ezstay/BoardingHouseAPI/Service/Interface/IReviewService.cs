using BoardingHouseAPI.DTO.Response;

namespace BoardingHouseAPI.Service.Interface
{
    public interface IReviewService
    {
        Task<List<ReviewResponse>?> GetReviewsByRoomsAsync(List<RoomResponse> rooms);
    }
}
