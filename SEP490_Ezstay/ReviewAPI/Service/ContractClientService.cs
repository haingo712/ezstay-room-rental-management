using ReviewAPI.DTO.Response;
using ReviewAPI.Service.Interface;

namespace ReviewAPI.Service;

public class ContractClientService: IContractClientService
{
    private readonly HttpClient _httpClient;

    public ContractClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> CheckTenantHasContract(Guid tenantId, Guid roomId)
    {
        var response = await _httpClient.GetAsync($"/api/Contract/HasContract/{tenantId}/roomId/{roomId}");
        if (!response.IsSuccessStatusCode) return false;

        var result = await response.Content.ReadAsStringAsync();
        return bool.Parse(result);
    }
    public async Task<ContractResponseDto?> GetContractById(Guid contractId)
    {
        var response = await _httpClient.GetAsync($"/api/Contract/{contractId}");
        if (!response.IsSuccessStatusCode) return null;

        // var wrapper = await response.Content.ReadFromJsonAsync<ApiResponse<ContractResponseDto>>(
        //     new System.Text.Json.JsonSerializerOptions
        //     {
        //         PropertyNameCaseInsensitive = true
        //     });
        //
        // return wrapper?.Data;
        var contract = await response.Content.ReadFromJsonAsync<ContractResponseDto>(
            new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        return contract;
    }
    
}