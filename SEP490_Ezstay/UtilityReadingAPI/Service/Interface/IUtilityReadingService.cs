
using Shared.DTOs;
using Shared.DTOs.UtilityReadings.Responses;
using UtilityReadingAPI.DTO.Request;
using UtilityReadingAPI.Enum;
using UtilityReadingAPI.Model;


namespace UtilityReadingAPI.Service.Interface;

public interface IUtilityReadingService
{
    // Task<IEnumerable<ElectricityReadingDto>> GetAllByOwnerId(Guid ownerId);
    IQueryable<UtilityReadingResponse> GetAllByOwnerId(Guid roomId,  UtilityType type);
    Task<UtilityReadingResponse> GetByIdAsync(Guid id);
    UtilityReadingResponse GetLastestReading(Guid roomId, UtilityType type);
    Task<ApiResponse<UtilityReadingResponse>> AddAsync(Guid roomId, CreateUtilityReading request);
    Task<ApiResponse<UtilityReadingResponse>> AddAsync(Guid roomId, UtilityType type, CreateUtilityReadingContract request); 
    Task<ApiResponse<UtilityReadingResponse>> AddUtilityReadingContract(Guid roomId, CreateUtilityReadingContract request);
    Task<ApiResponse<UtilityReadingResponse>> AddWater(Guid roomId, CreateUtilityReadingContract request);
    Task<ApiResponse<UtilityReadingResponse>> AddElectric(Guid roomId, CreateUtilityReadingContract request);
    Task<ApiResponse<bool>> UpdateWater(Guid roomId, UpdateUtilityReading request);
    Task<ApiResponse<bool>> UpdateElectric(Guid roomId, UpdateUtilityReading request);
    Task<ApiResponse<bool>> UpdateAsync(Guid id,UpdateUtilityReading request);
    Task DeleteAsync(Guid id);
    
}