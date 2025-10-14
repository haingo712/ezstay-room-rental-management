using System.Net.Http.Headers;
using System.Text.Json;
using AmenityAPI.APIs.Interfaces;
using Shared.DTOs;
using Shared.DTOs.Amenities.Responses;

namespace AmenityAPI.APIs;

public class ImageAPI:IImageAPI
{
        private readonly HttpClient _httpClient;

        public ImageAPI(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> UploadImage(IFormFile file)
        {
            using var form = new MultipartFormDataContent();
            var streamContent = new StreamContent(file.OpenReadStream());
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            form.Add(streamContent, "File", file.FileName);

            // Gọi sang ImageAPI
            var response = await _httpClient.PostAsync("/api/images/upload", form);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ImageResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result?.Url ?? string.Empty;
        }
}
