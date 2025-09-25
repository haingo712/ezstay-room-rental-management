using AccountAPI.Service.Interfaces;
using System.Text.Json;

namespace AccountAPI.Service
{
    public class AddressApiClient : IAddressApiClient
    {
        private readonly HttpClient _http;

        public AddressApiClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<string?> GetProvinceNameAsync(string provinceId)
        {
            var response = await _http.GetFromJsonAsync<JsonElement>("/api/provinces");
            var provinces = response.GetProperty("provinces").EnumerateArray();
            return provinces.FirstOrDefault(p => p.GetProperty("code").GetString() == provinceId)
                            .GetProperty("name").GetString();
        }

        public async Task<string?> GetCommuneNameAsync(string provinceId, string communeId)
        {
            try
            {
                var response = await _http.GetAsync($"/api/provinces/{provinceId}/communes");

                if (!response.IsSuccessStatusCode)
                    return null;

                var content = await response.Content.ReadFromJsonAsync<JsonElement>();

                if (!content.TryGetProperty("communes", out var communesElement))
                    return null;

                var communes = communesElement.EnumerateArray();
                var match = communes.FirstOrDefault(c =>
                    c.TryGetProperty("code", out var codeProp) && codeProp.GetString() == communeId);

                if (match.ValueKind != JsonValueKind.Undefined && match.TryGetProperty("name", out var nameProp))
                    return nameProp.GetString();

                return null;
            }
            catch
            {
                return null;
            }
        }

    }
}
