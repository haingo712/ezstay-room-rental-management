using BoardingHouseAPI.DTO.Request;
using BoardingHouseAPI.DTO.Response;
using BoardingHouseAPI.Enum;

namespace BoardingHouseAPI.Service.Interface
{
    public interface IBoardingHouseService
    {        
        IQueryable<BoardingHouseDTO> GetAll();
        IQueryable<BoardingHouseDTO> GetByOwnerId(Guid ownerId);        
        Task<BoardingHouseDTO?> GetByIdAsync(Guid id);
        Task<ApiResponse<BoardingHouseDTO>> CreateAsync(Guid ownerId, CreateBoardingHouseDTO createDto);
        Task<ApiResponse<bool>> UpdateAsync(Guid id, UpdateBoardingHouseDTO updateDto);
        Task<ApiResponse<bool>> DeleteAsync(Guid id);
        Task<List<BoardingHouseRankResponse>> GetRankedBoardingHousesAsync(RankType type, string order, int limit);
        Task<ApiResponse<SentimentSummaryResponse>> GetSentimentSummaryAsync(Guid boardingHouseId);
        Task<ApiResponse<RatingSummaryResponse>> GetRatingSummaryAsync(Guid boardingHouseId);
        Task<ApiResponse<OccupancyRateResponse>> GetOwnerOccupancyRateAsync(Guid ownerId);
    }
}
