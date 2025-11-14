using System.Text.Json;
using Shared.DTOs.Contracts.Responses;

namespace MailApi.APIs;

public class ContractAPI : IContractAPI
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public ContractAPI(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("ContractAPI");
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<ContractResponse?> GetContractByIdAsync(Guid contractId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/Contract/{contractId}");
            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ContractResponse>(content, _jsonOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error calling ContractAPI: {ex.Message}");
            return null;
        }
    }
}
