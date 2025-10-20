
using Shared.DTOs;
using Shared.DTOs.UtilityReadings.Responses;
using Shared.Enums;
using UtilityReadingAPI.DTO.Request;


namespace UtilityReadingAPI.Service.Interface;

public interface IUtilityReadingService
{
    // Task<IEnumerable<ElectricityReadingDto>> GetAllByOwnerId(Guid ownerId);
    IQueryable<UtilityReadingResponse> GetAllByOwnerId(Guid roomId,  UtilityType type);
    Task<UtilityReadingResponse> GetByIdAsync(Guid id);
    Task<UtilityReadingResponse> GetLastestReading(Guid roomId, UtilityType type);
    // Task<ApiResponse<UtilityReadingResponse>> AddAsync(Guid roomId, CreateUtilityReading request);
    Task<ApiResponse<UtilityReadingResponse>> AddAsync(Guid roomId, UtilityType type, CreateUtilityReadingContract request); 
    Task<ApiResponse<UtilityReadingResponse>> AddUtilityReadingContract(Guid roomId,UtilityType type, CreateUtilityReadingContract request);
    Task<ApiResponse<bool>> UpdateContract(Guid roomId, UtilityType type, UpdateUtilityReading request);
    Task<ApiResponse<bool>> UpdateAsync(Guid id,UpdateUtilityReading request);
    Task DeleteAsync(Guid id);
    
}