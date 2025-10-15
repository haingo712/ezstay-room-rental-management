using RoomAPI.Service.Interface;
using Shared.DTOs;

namespace RoomAPI.Service;

public class ContractClientService : IContractClientService
{
    private readonly HttpClient _httpClient;

    public ContractClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> HasContractByRoomId(Guid roomId)
    {
        var response = await _httpClient.GetAsync($"api/Contract/room/{roomId}/exists");
        if (!response.IsSuccessStatusCode)
            return false;

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();
        return result?.Data ?? false;
    }
}
