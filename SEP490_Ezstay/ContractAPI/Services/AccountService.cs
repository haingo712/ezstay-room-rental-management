using ContractAPI.DTO.Response;
using ContractAPI.Services.Interfaces;
using Shared.DTOs.Contracts.Responses;

namespace ContractAPI.Services;

public class AccountService:IAccountService
{
    private readonly HttpClient _httpClient;

    public AccountService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    
    public async Task<AccountResponse> GetProfileByUserId(Guid userId)
    {
            var response = await _httpClient.GetAsync($"api/User/profile/{userId}");
            return await response.Content.ReadFromJsonAsync<AccountResponse>();
    }

}