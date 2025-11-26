using Shared.DTOs.UtilityReadings.Responses;
using Shared.Enums;
using UtilityBillAPI.Service.Interface;

namespace UtilityBillAPI.Service
{
    public class UtilityReadingService : IUtilityReadingService
    {
        private readonly HttpClient _httpClient;

        public UtilityReadingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private async Task<UtilityReadingResponse?> GetLatestReadingAsync(Guid contractId, UtilityType type)
        {
            var requestUrl = $"api/UtilityReading/latest/{contractId}/{type}";
            var response = await _httpClient.GetAsync(requestUrl);

            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<UtilityReadingResponse>()
                : null;
        }

        public Task<UtilityReadingResponse?> GetElectricityReadingAsync(Guid contractId) =>
            GetLatestReadingAsync(contractId, UtilityType.Electric);

        public Task<UtilityReadingResponse?> GetWaterReadingAsync(Guid contractId) =>
            GetLatestReadingAsync(contractId, UtilityType.Water);

        public async Task<List<UtilityReadingResponse>> GetUtilityReadingsAsync(Guid contractId)
        {
            var electricity = await GetElectricityReadingAsync(contractId);
            var water = await GetWaterReadingAsync(contractId);

            var list = new List<UtilityReadingResponse>();

            if (electricity != null) list.Add(electricity);
            if (water != null) list.Add(water);

            return list;
        }
    }

}
