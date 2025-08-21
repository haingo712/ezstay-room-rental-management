
using UtilityReadingAPI.DTO.Request;
using UtilityReadingAPI.DTO.Response;
using UtilityReadingAPI.Model;


namespace UtilityReadingAPI.Service.Interface;

public interface IUtilityReadingService
{
    // Task<IEnumerable<ElectricityReadingDto>> GetAllByOwnerId(Guid ownerId);
  //  IQueryable<ElectricityReadingDto> GetAllByOwnerIdOdata(Guid ownerId);
    Task<UtilityReadingDto> GetByIdAsync(Guid id);
    Task<ApiResponse<UtilityReadingDto>> AddAsync(CreateUtilityReadingDto request);
    Task<ApiResponse<bool>> UpdateAsync(Guid id,UpdateUtilityReadingDto request);
   // Task DeleteAsync(Guid id);
    
}