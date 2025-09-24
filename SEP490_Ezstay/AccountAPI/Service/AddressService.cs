using AccountAPI.Service.Interfaces;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace AccountAPI.Service
{
    public class AddressService : IAddressApiClient
    {
        private readonly HttpClient _httpClient;
        private JsonElement _allData = default;

        public AddressService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task LoadAsync()
        {
            if (_allData.ValueKind == JsonValueKind.Array) return;

            var response = await _httpClient.GetFromJsonAsync<JsonElement>("api/?depth=3");
            if (response.ValueKind == JsonValueKind.Array)
            {
                _allData = response;
            }
        }

        public string? GetProvinceName(string code)
        {
            if (_allData.ValueKind != JsonValueKind.Array) return null;

            var province = _allData.EnumerateArray()
                .FirstOrDefault(p => p.GetProperty("code").GetInt32().ToString("D2") == code);

            return province.ValueKind == JsonValueKind.Object
                ? province.GetProperty("name").GetString()
                : null;
        }

        public string? GetCommuneName(string provinceCode, string communeCode)
        {
            if (_allData.ValueKind != JsonValueKind.Array) return null;

            var province = _allData.EnumerateArray()
                .FirstOrDefault(p => p.GetProperty("code").GetInt32().ToString("D2") == provinceCode);

            if (province.ValueKind != JsonValueKind.Object || !province.TryGetProperty("districts", out var districts))
                return null;

            foreach (var district in districts.EnumerateArray())
            {
                if (!district.TryGetProperty("wards", out var wards)) continue;

                foreach (var ward in wards.EnumerateArray())
                {
                    if (ward.TryGetProperty("code", out var codeProp) &&
                        ward.TryGetProperty("name", out var nameProp) &&
                        codeProp.GetInt32().ToString("D5") == communeCode) // ✅ Sửa ở đây
                    {
                        return nameProp.GetString();
                    }
                }
            }

            return null;
        }

    }
}
