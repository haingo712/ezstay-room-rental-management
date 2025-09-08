using BoardingHouseAPI.DTO.Request;
using BoardingHouseAPI.DTO.Response;

namespace BoardingHouseAPI.Service.Interface
{
    public interface IBoardingHouseService
    {        
        IQueryable<BoardingHouseDTO> GetAll();
        IQueryable<BoardingHouseDTO> GetByOwnerId(Guid ownerId);
        IQueryable<BoardingHouseDTO> GetByOwnerId(); 
        Task<BoardingHouseDTO?> GetByIdAsync(Guid id);
        Task<ApiResponse<BoardingHouseDTO>> CreateAsync(CreateBoardingHouseDTO createDto);
        Task<ApiResponse<bool>> UpdateAsync(Guid id, UpdateBoardingHouseDTO updateDto);
        Task<ApiResponse<bool>> DeleteAsync(Guid id);
    }
}
