using System.Net.Http.Headers;
using System.Text.Json;
using AmenityAPI.Service.Interface;
using Shared.DTOs;

namespace AmenityAPI.Service;

public class ImageService:IImageService
{
        private readonly HttpClient _httpClient;

        public ImageService(HttpClient httpClient)
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
