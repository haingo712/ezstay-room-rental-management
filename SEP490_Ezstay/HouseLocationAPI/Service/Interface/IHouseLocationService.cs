using HouseLocationAPI.DTO.Request;
using HouseLocationAPI.DTO.Response;

namespace HouseLocationAPI.Service.Interface
{
    public interface IHouseLocationService
    {        
        IQueryable<HouseLocationDTO> GetAll();
        IQueryable<HouseLocationDTO> GetByHouseId(Guid houseId);
        Task<HouseLocationDTO?> GetByIdAsync(Guid id);
        Task<ApiResponse<HouseLocationDTO>> CreateAsync(CreateHouseLocationDTO createDto);
        Task<ApiResponse<bool>> UpdateAsync(Guid id, UpdateHouseLocationDTO updateDto);
        Task<ApiResponse<bool>> DeleteAsync(Guid id);
    }
}
