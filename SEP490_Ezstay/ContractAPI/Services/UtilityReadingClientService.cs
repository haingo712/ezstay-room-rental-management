
using System.Net.Http.Headers;
using ContractAPI.DTO.Requests.UtilityReading;
using ContractAPI.DTO.Response;
using ContractAPI.Services.Interfaces;

namespace ContractAPI.Services;

public class UtilityReadingClientService : IUtilityReadingClientService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UtilityReadingClientService(HttpClient httpClient, IHttpContextAccessor accessor)
    {

        _httpClient = httpClient;
        _httpContextAccessor = accessor;
    }

    public async Task<ApiResponse<UtilityReadingResponseDto>> AddElectric(Guid roomId, CreateUtilityReadingContract request)
    {
        var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
        if (!string.IsNullOrEmpty(token))
            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token.Replace("Bearer ", ""));

        var response = await _httpClient.PostAsJsonAsync($"api/UtilityReading/{roomId}/electric", request);

        if (!response.IsSuccessStatusCode)
        {
            return ApiResponse<UtilityReadingResponseDto>.Fail(
                $"UtilityReading API call failed: {response.StatusCode}");
        }
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<UtilityReadingResponseDto>>();
        return result!;
    }
    public async Task<ApiResponse<UtilityReadingResponseDto>> AddWater(Guid roomId, CreateUtilityReadingContract request)
    {
        var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
        if (!string.IsNullOrEmpty(token))
            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token.Replace("Bearer ", ""));

        var response = await _httpClient.PostAsJsonAsync($"api/UtilityReading/{roomId}/water", request);

        if (!response.IsSuccessStatusCode)
        {
            return ApiResponse<UtilityReadingResponseDto>.Fail(
                $"UtilityReading API call failed: {response.StatusCode}");
        }
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<UtilityReadingResponseDto>>();
        return result!;
    }
    
}