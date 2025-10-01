
using UtilityReadingAPI.DTO.Request;
using UtilityReadingAPI.DTO.Response;
using UtilityReadingAPI.Enum;
using UtilityReadingAPI.Model;


namespace UtilityReadingAPI.Service.Interface;

public interface IUtilityReadingService
{
    // Task<IEnumerable<ElectricityReadingDto>> GetAllByOwnerId(Guid ownerId);
    IQueryable<UtilityReadingResponseDto> GetAllByOwnerId(Guid roomId,  UtilityType type);
    Task<UtilityReadingResponseDto> GetByIdAsync(Guid id);
    UtilityReadingResponseDto GetLastestReading(Guid roomId, UtilityType type);
    Task<ApiResponse<UtilityReadingResponseDto>> AddAsync(Guid roomId, CreateUtilityReadingDto request);
    Task<ApiResponse<UtilityReadingResponseDto>> AddAsync(Guid roomId, UtilityType type, CreateUtilityReadingContract request); 
    Task<ApiResponse<UtilityReadingResponseDto>> AddUtilityReadingContract(Guid roomId, CreateUtilityReadingContract request);
    Task<ApiResponse<UtilityReadingResponseDto>> AddWater(Guid roomId, CreateUtilityReadingContract request);
    Task<ApiResponse<UtilityReadingResponseDto>> AddElectric(Guid roomId, CreateUtilityReadingContract request);

    Task<ApiResponse<bool>> UpdateAsync(Guid id,UpdateUtilityReadingDto request);
    Task DeleteAsync(Guid id);
    
}