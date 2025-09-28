
using ContractAPI.DTO.Requests.UtilityReading;
using ContractAPI.DTO.Response;
using ContractAPI.Services.Interfaces;

namespace ContractAPI.Services;

public class UtilityReadingClientService : IUtilityReadingClientService
{
    private readonly HttpClient _httpClient;

    public UtilityReadingClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ApiResponse<UtilityReadingResponseDto>> AddAsync(Guid roomId, CreateWaterDto request)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/UtilityReading/{roomId}", request);

        if (!response.IsSuccessStatusCode)
        {
            return ApiResponse<UtilityReadingResponseDto>.Fail(
                $"UtilityReading API call failed: {response.StatusCode}");
        }
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<UtilityReadingResponseDto>>();
        return result!;
    }
}