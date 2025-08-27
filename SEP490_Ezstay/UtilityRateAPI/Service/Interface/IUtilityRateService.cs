using UtilityRateAPI.DTO.Request;
using UtilityRateAPI.DTO.Response;
using UtilityRateAPI.DTO.Request;

namespace UtilityRateAPI.Service.Interface;



public interface IUtilityRateService
{
    // Task<ApiResponse<IEnumerable<UtilityRateDto>>> GetAllByOwnerId(Guid ownerId);
    // IQueryable<UtilityRateDto> GetAllByOwnerIdOdata(Guid ownerId);
    // IQueryable<UtilityRateDto> GetAllOdata();
    // Task<ApiResponse<IEnumerable<UtilityRateDto>>> GetAll();
    // Task<UtilityRateDto> GetByIdAsync(Guid id);
    // Task<ApiResponse<UtilityRateDto>> AddAsync(CreateUtilityRateDto request);
    // Task<ApiResponse<bool>> UpdateAsync(Guid id,UpdateUtilityRateDto request);
    // Task DeleteAsync(Guid id);
    Task<ApiResponse<IEnumerable<UtilityRateDto>>> GetAllByOwnerId( );
    IQueryable<UtilityRateDto> GetAllByOwnerIdOdata( );
    IQueryable<UtilityRateDto> GetAllOdata();
    Task<ApiResponse<IEnumerable<UtilityRateDto>>> GetAll();
    Task<UtilityRateDto> GetByIdAsync(Guid id);
    Task<ApiResponse<UtilityRateDto>> AddAsync(CreateUtilityRateDto request);
    Task<ApiResponse<bool>> UpdateAsync(Guid id,UpdateUtilityRateDto request);
    Task DeleteAsync(Guid id);
    
}