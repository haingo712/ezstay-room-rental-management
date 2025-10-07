using System.Net.Http.Headers;
using System.Text.Json;
using ContractAPI.APIs.Interfaces;
using ContractAPI.DTO.Response;

namespace ContractAPI.APIs;

public class ImageAPI:IImageAPI
{
        private readonly HttpClient _httpClient;

        public ImageAPI(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            using var form = new MultipartFormDataContent();
            var streamContent = new StreamContent(file.OpenReadStream());
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            form.Add(streamContent, "File", file.FileName);

            // Gọi sang ImageAPI
            var response = await _httpClient.PostAsync("/api/images/upload", form);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<UploadResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result?.Url ?? string.Empty;
        }
}
