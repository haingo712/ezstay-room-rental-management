
using ContractAPI.DTO.Requests.UtilityReading;
using ContractAPI.DTO.Response;

namespace ContractAPI.Services.Interfaces;

public interface IUtilityReadingClientService
{
    Task<ApiResponse<UtilityReadingResponseDto>> AddAsync(Guid roomId, CreateWaterDto request);
}