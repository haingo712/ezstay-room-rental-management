
using ContractAPI.DTO.Requests.UtilityReading;
using ContractAPI.DTO.Response;
using Shared.DTOs;
using Shared.DTOs.UtilityReadings.Responses;
using Shared.Enums;

namespace ContractAPI.Services.Interfaces;

public interface IUtilityReadingService
{
    Task<ApiResponse<UtilityReadingResponse>> Add(Guid contractId, UtilityType utilityType, CreateUtilityReadingContract request);
    Task<ApiResponse<bool>> Update(Guid contractId, UtilityType type, UpdateUtilityReading request);
   
    Task<ApiResponse<UtilityReadingResponse>> GetById(Guid id);
    Task<UtilityReadingResponse> GetLastestReading(Guid contractId, UtilityType utilityType);

}