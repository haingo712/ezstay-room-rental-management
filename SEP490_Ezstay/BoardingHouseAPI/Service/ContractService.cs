using BoardingHouseAPI.Service.Interface;
using Shared.DTOs;

namespace BoardingHouseAPI.Service;

public class ContractService : IContractService
{
    private readonly HttpClient _httpClient;

    public ContractService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> ContractExistsByRoomId(Guid roomId)
    {
        var response = await _httpClient.GetAsync($"api/Contract/room/{roomId}/exists");
        if (!response.IsSuccessStatusCode)
            return false;

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();
        return result?.Data ?? false;
    }
}
