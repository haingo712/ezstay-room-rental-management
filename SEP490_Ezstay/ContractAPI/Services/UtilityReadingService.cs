
using System.Net.Http.Headers;
using ContractAPI.DTO.Requests.UtilityReading;
using ContractAPI.Services.Interfaces;
using Shared.DTOs;
using Shared.DTOs.UtilityReadings.Responses;
using Shared.Enums;

namespace ContractAPI.Services;

public class UtilityReadingService : IUtilityReadingService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UtilityReadingService(HttpClient httpClient, IHttpContextAccessor accessor)
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
    public async Task<ApiResponse<UtilityReadingResponse>> Add(Guid contractId, UtilityType utilityType, CreateUtilityReadingContract request)
    {
        // var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
        // if (!string.IsNullOrEmpty(token))
        //     _httpClient.DefaultRequestHeaders.Authorization = 
        //         new AuthenticationHeaderValue("Bearer", token.Replace("Bearer ", ""));

        var url = $"api/UtilityReading/{contractId}/utilitytype/{utilityType}/contract";
        var response = await _httpClient.PostAsJsonAsync(url, request);

        if (!response.IsSuccessStatusCode)
        {
            return ApiResponse<UtilityReadingResponse>.Fail(
                $"UtilityReading API call failed: {response.StatusCode}");
        }
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<UtilityReadingResponse>>();
        return result!;
    }
    
    public async Task<ApiResponse<bool>> Update(Guid contractId, UtilityType utilityType, UpdateUtilityReading request)
    {
      //  AttachBearerTokenIfExists();

        var response = await _httpClient.PutAsJsonAsync($"api/UtilityReading/{contractId}/utilitytype/{utilityType}/contract", request);

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

    public async Task<UtilityReadingResponse> GetLastestReading(Guid contractId, UtilityType utilityType)
    {
        var response = await _httpClient.GetAsync($"api/UtilityReading/latest/{contractId}/{utilityType}");
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"UtilityReading API call failed: {response.StatusCode}");
            return null;
        }

        var result =await response.Content.ReadFromJsonAsync<UtilityReadingResponse>();
         return result;
    }
}