using Shared.DTOs.UtilityReadings.Responses;
using UtilityBillAPI.Service.Interface;

namespace UtilityBillAPI.Service
{
    public class UtilityReadingClientService : IUtilityReadingClientService
    {
        private readonly HttpClient _httpClient;
        public UtilityReadingClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UtilityReadingResponse?> GetElectricityReadingAsync(Guid roomId)
        {
            var requestUrl = $"api/UtilityReading/latest/{roomId}/Electric";
            var response = await _httpClient.GetAsync(requestUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UtilityReadingResponse>();
            }
            return null;
        }

        public async Task<UtilityReadingResponse?> GetWaterReadingAsync(Guid roomId)
        {
            var requestUrl = $"api/UtilityReading/latest/{roomId}/Water";
            var response = await _httpClient.GetAsync(requestUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UtilityReadingResponse>();
            }
            return null;
        }

        // Get utility readings for a specific room and check speriod start and end dates
        public async Task<List<UtilityReadingResponse>?> GetUtilityReadingsAsync(Guid roomId, DateTime startDate, DateTime endDate)
        {
            var readings = new List<UtilityReadingResponse>();
            var lastestElectricReading = await GetElectricityReadingAsync(roomId);
            var lastestWaterReading = await GetWaterReadingAsync(roomId);

            if (lastestElectricReading != null &&
                lastestElectricReading.ReadingDate >= startDate &&
                lastestElectricReading.ReadingDate <= endDate)
            {
                readings.Add(lastestElectricReading);
            } 

            if (lastestWaterReading != null &&
                lastestWaterReading.ReadingDate >= startDate &&
                lastestWaterReading.ReadingDate <= endDate)
            {
                readings.Add(lastestWaterReading);
            }

            return readings;
        }

    }
}
