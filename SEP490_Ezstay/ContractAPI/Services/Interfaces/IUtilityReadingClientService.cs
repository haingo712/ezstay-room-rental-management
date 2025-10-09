
using ContractAPI.DTO.Requests.UtilityReading;
using ContractAPI.DTO.Response;
using Shared.DTOs;
using Shared.DTOs.UtilityReadings.Responses;
using Shared.Enums;

namespace ContractAPI.Services.Interfaces;

public interface IUtilityReadingClientService
{
    Task<ApiResponse<UtilityReadingResponse>> Add(Guid ownerId, UtilityType utilityType, CreateUtilityReadingContract request);
    Task<ApiResponse<bool>> Update(Guid roomId, UtilityType type, UpdateUtilityReading request);
   
    Task<ApiResponse<UtilityReadingResponse>> GetById(Guid id);
}