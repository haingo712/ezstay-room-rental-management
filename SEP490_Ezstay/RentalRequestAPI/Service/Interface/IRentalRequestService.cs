using RentalRequestAPI.DTO.Request;
using RentalRequestAPI.DTO.Response;

namespace RentalRequestAPI.Service.Interface;

public interface IRentalRequestService
{
    Task<IEnumerable<RentalRequestDto>> GetAllByStaffId(Guid staffId);
    IQueryable<RentalRequestDto> GetAllByStaffIdOdata(Guid staffId);
    IQueryable<RentalRequestDto> GetAllOdata();
    Task<ApiResponse<IEnumerable<RentalRequestDto>>> GetAll();
    Task<RentalRequestDto> GetByIdAsync(Guid id);
    Task<ApiResponse<RentalRequestDto>> AddAsync(CreateRentalRequestDto request);
    Task<ApiResponse<bool>> UpdateAsync(Guid id,UpdateRentalRequestDto request);
    Task DeleteAsync(Guid id);
    
}