using System.Net.Http.Headers;
using ContractAPI.DTO.Response;
using ContractAPI.Services.Interfaces;
using Newtonsoft.Json;
using Shared.DTOs.Contracts.Responses;

namespace ContractAPI.Services;

public class AccountService:IAccountService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly HttpClient _httpClient;

    public AccountService(IHttpContextAccessor httpContextAccessor, HttpClient httpClient) {
        _httpContextAccessor = httpContextAccessor;
        _httpClient = httpClient;
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
    public async Task<ProfileResponse> GetProfileByUserId(Guid userId)
    {
      //  AttachBearerTokenIfExists();
      // var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
      // if (!string.IsNullOrEmpty(token))
      //     _httpClient.DefaultRequestHeaders.Authorization = 
      //         new AuthenticationHeaderValue("Bearer", token.Replace("Bearer ", ""));
            var response = await _httpClient.GetAsync($"api/Profile/profile/{userId}");
            // if (!response.IsSuccessStatusCode)
            // {
            //     // Log hoặc throw tùy mục đích
            //     var content = await response.Content.ReadAsStringAsync();
            //     Console.WriteLine($"❌ Lỗi khi gọi API profile: {response.StatusCode} - {content}");
            //     return null;
            // }
            //
            // //return await response.Content.ReadFromJsonAsync<AccountResponse>();
            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ProfileResponse>(content);
    }

}