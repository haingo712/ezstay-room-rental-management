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
}