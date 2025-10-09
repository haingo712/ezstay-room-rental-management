
using ContractAPI.DTO.Requests.UtilityReading;
using ContractAPI.DTO.Response;
using Shared.DTOs;

namespace ContractAPI.Services.Interfaces;

public interface IUtilityReadingClientService
{
    Task<ApiResponse<UtilityReadingResponseDto>> AddElectric(Guid roomId, CreateUtilityReadingContract request);
    Task<ApiResponse<UtilityReadingResponseDto>> AddWater(Guid roomId, CreateUtilityReadingContract request);
    Task<ApiResponse<bool>> UpdateElectric(Guid roomId, UpdateUtilityReading request);
    Task<ApiResponse<bool>> UpdateWater(Guid roomId, UpdateUtilityReading request);
    Task<ApiResponse<UtilityReadingResponseDto>> GetById(Guid id);
}