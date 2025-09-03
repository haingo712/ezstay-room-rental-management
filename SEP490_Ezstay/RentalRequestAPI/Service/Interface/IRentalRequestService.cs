using RentalRequestAPI.DTO.Request;
using RentalRequestAPI.DTO.Response;

namespace RentalRequestAPI.Service.Interface;

public interface IRentalRequestService
{
  //  Task<List<RentalRequestDto>> GetAllByOwnerId(Guid ownerId);
   // Task<List<RentalRequestDto>> GetAllByUserId(Guid userId);
    IQueryable<RentalRequestDto> GetAllByUserIdOdata(Guid userId);
    IQueryable<RentalRequestDto> GetAllByOwnerIdOdata(Guid ownerId);
   
    // IQueryable<RentalRequestDto> GetAllOdata();
   // Task<ApiResponse<IEnumerable<RentalRequestDto>>> GetAll();
    Task<RentalRequestDto> GetByIdAsync(Guid id);
    Task<ApiResponse<RentalRequestDto>> AddAsync(CreateRentalRequestDto request);
    Task<ApiResponse<bool>> UpdateAsync(Guid id,UpdateRentalRequestDto request);
  //  Task DeleteAsync(Guid id);
    
}