using System.Net.Http.Headers;
using ReviewAPI.DTO.Response;
using ReviewAPI.Service.Interface;

namespace ReviewAPI.Service;

public class ContractService: IContractService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ContractService(HttpClient client, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = client;
        _httpContextAccessor = httpContextAccessor;
    }
    // public async Task<bool> CheckTenantHasContract(Guid tenantId, Guid roomId)
    // {
    //     var response = await _httpClient.GetAsync($"/api/Contract/HasContract/{tenantId}/roomId/{roomId}");
    //     if (!response.IsSuccessStatusCode) return false;
    //     var result = await response.Content.ReadAsStringAsync();
    //     return bool.Parse(result);
    // }
   

    public async Task<ContractResponse> GetContractId(Guid contractId)
    {
        var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();

        if (!string.IsNullOrEmpty(token))
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token.Replace("Bearer ", ""));

        var response = await _httpClient.GetAsync($"/api/Contract/{contractId}");
        if (!response.IsSuccessStatusCode) return null;
        var contract = await response.Content.ReadFromJsonAsync<ContractResponse>(
            new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        return contract;
    }
    
}