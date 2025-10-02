using UtilityBillAPI.DTO.Request;
using UtilityBillAPI.Service.Interface;

namespace UtilityBillAPI.Service
{
    public class UtilityReadingClient : IUtilityReadingClient
    {
        private readonly HttpClient _httpClient;
        public UtilityReadingClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UtilityReadingDTO?> GetElectricityReadingAsync(Guid roomId)
        {
            var requestUrl = $"api/UtilityReading/lastest/{roomId}?utilityType=Electric";
            var response = await _httpClient.GetAsync(requestUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UtilityReadingDTO>();
            }
            return null;
        }

        public async Task<UtilityReadingDTO?> GetWaterReadingAsync(Guid roomId)
        {
            var requestUrl = $"api/UtilityReading/lastest/{roomId}?utilityType=Water";
            var response = await _httpClient.GetAsync(requestUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UtilityReadingDTO>();
            }
            return null;
        }

    }
}
