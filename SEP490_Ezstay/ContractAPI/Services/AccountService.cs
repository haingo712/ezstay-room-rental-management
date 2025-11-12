using System.Net.Http.Headers;
using ContractAPI.DTO.Response;
using ContractAPI.Services.Interfaces;
using Newtonsoft.Json;
using Shared.DTOs.Contracts.Responses;

namespace ContractAPI.Services;

public class AccountService(IHttpContextAccessor _httpContextAccessor,HttpClient _httpClient):IAccountService
{


    private void AttachBearerTokenIfExists()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null) return;

        var token = context.Request.Headers["Authorization"].ToString();
        if (!string.IsNullOrEmpty(token))
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token.Replace("Bearer ", ""));
    } 
    public async Task<AccountResponse> GetProfileByUserId(Guid userId)
    {
      //  AttachBearerTokenIfExists();
      // var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
      // if (!string.IsNullOrEmpty(token))
      //     _httpClient.DefaultRequestHeaders.Authorization = 
      //         new AuthenticationHeaderValue("Bearer", token.Replace("Bearer ", ""));
            var response = await _httpClient.GetAsync($"api/User/profile/{userId}");
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
            return JsonConvert.DeserializeObject<AccountResponse>(content);
    }

}