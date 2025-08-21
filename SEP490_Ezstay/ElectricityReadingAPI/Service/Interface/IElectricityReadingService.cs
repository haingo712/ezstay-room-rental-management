
using ElectricityReadingAPI.DTO.Request;
using ElectricityReadingAPI.DTO.Response;


namespace ElectricityReadingAPI.Service.Interface;

public interface IElectricityReadingService
{
    // Task<IEnumerable<ElectricityReadingDto>> GetAllByOwnerId(Guid ownerId);
  //  IQueryable<ElectricityReadingDto> GetAllByOwnerIdOdata(Guid ownerId);
    Task<ElectricityReadingDto> GetByIdAsync(Guid id);
    Task<ApiResponse<ElectricityReadingDto>> AddAsync(CreateElectricityReadingDto request);
    Task<ApiResponse<bool>> UpdateAsync(Guid id,UpdateElectricityReadingDto request);
   // Task DeleteAsync(Guid id);
    
}