
using ContractAPI.DTO.Requests.UtilityReading;
using ContractAPI.DTO.Response;

namespace ContractAPI.Services.Interfaces;

public interface IUtilityReadingClientService
{
    Task<ApiResponse<UtilityReadingResponseDto>> AddElectric(Guid roomId, CreateUtilityReadingContract request);
    Task<ApiResponse<UtilityReadingResponseDto>> AddWater(Guid roomId, CreateUtilityReadingContract request);
}