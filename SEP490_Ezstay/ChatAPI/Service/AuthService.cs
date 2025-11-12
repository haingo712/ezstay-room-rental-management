using ChatAPI.Service.Interface;
using Shared.DTOs;
using Shared.DTOs.Auths.Responses;

namespace ChatAPI.Service;

public class AuthService:IAuthService
{
    private readonly HttpClient _httpClient;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<AccountResponse?> GetById(Guid accountId)
    {
        var response = await _httpClient.GetAsync($"api/Accounts/{accountId}");

        // if (!response.IsSuccessStatusCode)
        // {
        //     return null;
        // }
        var apiResponse = await response.Content.ReadFromJsonAsync<AccountResponse>();
        return apiResponse;
    }
}