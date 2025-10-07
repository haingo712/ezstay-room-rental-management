using ContractAPI.APIs.Interfaces;
using ContractAPI.DTO.Response;

namespace ContractAPI.APIs;

public class AccountAPI:IAccountAPI
{
    private readonly HttpClient _httpClient;

    public AccountAPI(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IdentityProfileResponseDto?> GetProfileByPhoneAsync(string phone)
    {
        var response = await _httpClient.GetAsync($"pi/user/searchphone/{phone}");
        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<IdentityProfileResponseDto>();
    }
}