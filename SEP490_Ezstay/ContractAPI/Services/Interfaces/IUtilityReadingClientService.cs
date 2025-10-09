
using ContractAPI.DTO.Requests.UtilityReading;
using ContractAPI.DTO.Response;
using Shared.DTOs;
using Shared.DTOs.UtilityReadings.Responses;
using Shared.Enums;

namespace ContractAPI.Services.Interfaces;

public interface IUtilityReadingClientService
{
    Task<ApiResponse<UtilityReadingResponse>> Add(Guid ownerId, UtilityType utilityType, CreateUtilityReadingContract request);
    Task<ApiResponse<UtilityReadingResponse>> AddElectric(Guid roomId, CreateUtilityReadingContract request);
    Task<ApiResponse<UtilityReadingResponse>> AddWater(Guid roomId, CreateUtilityReadingContract request);
    Task<ApiResponse<bool>> UpdateElectric(Guid roomId, UpdateUtilityReading request);
    Task<ApiResponse<bool>> UpdateWater(Guid roomId, UpdateUtilityReading request);
    Task<ApiResponse<UtilityReadingResponse>> GetById(Guid id);
}