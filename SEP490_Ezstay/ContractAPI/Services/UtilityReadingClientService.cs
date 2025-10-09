
using System.Net.Http.Headers;
using ContractAPI.DTO.Requests.UtilityReading;
using ContractAPI.DTO.Response;
using ContractAPI.Services.Interfaces;
using Shared.DTOs;
using Shared.DTOs.UtilityReadings.Responses;

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
    private void AttachBearerTokenIfExists()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null) return;

        var token = context.Request.Headers["Authorization"].ToString();
        if (!string.IsNullOrEmpty(token))
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token.Replace("Bearer ", ""));
    }


    public async Task<ApiResponse<UtilityReadingResponse>> AddElectric(Guid roomId, CreateUtilityReadingContract request)
    {
        var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
        if (!string.IsNullOrEmpty(token))
            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token.Replace("Bearer ", ""));

        var response = await _httpClient.PostAsJsonAsync($"api/UtilityReading/{roomId}/electric", request);

        if (!response.IsSuccessStatusCode)
        {
            return ApiResponse<UtilityReadingResponse>.Fail(
                $"UtilityReading API call failed: {response.StatusCode}");
        }
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<UtilityReadingResponse>>();
        return result!;
    }
    public async Task<ApiResponse<UtilityReadingResponse>> AddWater(Guid roomId, CreateUtilityReadingContract request)
    {
        var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
        if (!string.IsNullOrEmpty(token))
            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token.Replace("Bearer ", ""));

        var response = await _httpClient.PostAsJsonAsync($"api/UtilityReading/{roomId}/water", request);

        if (!response.IsSuccessStatusCode)
        {
            return ApiResponse<UtilityReadingResponse>.Fail(
                $"UtilityReading API call failed: {response.StatusCode}");
        }
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<UtilityReadingResponse>>();
        return result!;
    }
    public async Task<ApiResponse<bool>> UpdateElectric(Guid roomId, UpdateUtilityReading request)
    {
        AttachBearerTokenIfExists();

        var response = await _httpClient.PutAsJsonAsync($"api/UtilityReading/{roomId}/electric", request);

        if (!response.IsSuccessStatusCode)
            return ApiResponse<bool>.Fail($"UtilityReading API call failed: {response.StatusCode}");

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();
        return result!;
    }
    public async Task<ApiResponse<bool>> UpdateWater(Guid roomId, UpdateUtilityReading request)
    {
        AttachBearerTokenIfExists();

        var response = await _httpClient.PutAsJsonAsync($"api/UtilityReading/{roomId}/water", request);

        if (!response.IsSuccessStatusCode)
            return ApiResponse<bool>.Fail($"UtilityReading API call failed: {response.StatusCode}");

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();
        return result!;
    }
    public async Task<ApiResponse<UtilityReadingResponse>> GetById(Guid id)
    {
        AttachBearerTokenIfExists();
        var response = await _httpClient.GetAsync($"api/UtilityReading/{id}");

        if (!response.IsSuccessStatusCode)
            return ApiResponse<UtilityReadingResponse>.Fail($"UtilityReading API call failed: {response.StatusCode}");

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<UtilityReadingResponse>>();
        return result!;
    }
}