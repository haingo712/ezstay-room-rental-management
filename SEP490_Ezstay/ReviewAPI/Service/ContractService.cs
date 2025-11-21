using ReviewAPI.DTO.Response;
using ReviewAPI.Service.Interface;
using Shared.DTOs.Contracts.Responses;

namespace ReviewAPI.Service;

public class ContractService: IContractService
{
    private readonly HttpClient _httpClient;

    public ContractService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // public async Task<bool> CheckTenantHasContract(Guid tenantId, Guid roomId)
    // {
    //     var response = await _httpClient.GetAsync($"/api/Contract/HasContract/{tenantId}/roomId/{roomId}");
    //     if (!response.IsSuccessStatusCode) return false;
    //     var result = await response.Content.ReadAsStringAsync();
    //     return bool.Parse(result);
    // }
    public async Task<ContractResponse?> GetContractId(Guid contractId)
    {
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