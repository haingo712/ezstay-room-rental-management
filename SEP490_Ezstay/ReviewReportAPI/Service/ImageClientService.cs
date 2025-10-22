using ReviewReportAPI.Service.Interface;
using System.Net.Http.Headers;
using System.Text.Json;
using Shared.DTOs;
using Shared.DTOs.Images.Responses;

namespace ReviewReportAPI.Service;

 public class ImageClientService : IImageClientService
    {       
        private readonly HttpClient _httpClient;
        public ImageClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public async Task<string> UploadImage(IFormFile file)
        {
            var stream = file.OpenReadStream();
            using var form = new MultipartFormDataContent();
            var streamContent = new StreamContent(stream);

            streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            form.Add(streamContent, "File", file.FileName);
           
            var response = await _httpClient.PostAsync("api/Images/upload", form);

            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ImageResponse>(json, 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return result?.Url ?? string.Empty;
        }

        public async Task<List<string>> UploadMultipleImage(IFormFileCollection files)
        {
            using var form = new MultipartFormDataContent();
            foreach (var file in files)
            {
                var stream = file.OpenReadStream();
                var streamContent = new StreamContent(stream);
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                form.Add(streamContent, "Files", file.FileName);
            }
            var response = await _httpClient.PostAsync("api/Images/upload-multiple", form);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ImageMultipleResponse>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return result?.Urls ?? new List<string>();
        }
}
