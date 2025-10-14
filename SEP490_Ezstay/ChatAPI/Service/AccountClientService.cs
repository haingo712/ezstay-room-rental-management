using ChatAPI.Service.Interface;
using Shared.DTOs;
using Shared.DTOs.Accounts.Responses;

namespace ChatAPI.Service;

public class AccountClientService:IAccountClientService
{
    private readonly HttpClient _httpClient;

    public AccountClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<AccountResponse?> GetByIdAsync(Guid accountId)
    {
        var response = await _httpClient.GetAsync($"api/Accounts/{accountId}");

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        var apiResponse = await response.Content.ReadFromJsonAsync<AccountResponse>();
        return apiResponse;
    }
}