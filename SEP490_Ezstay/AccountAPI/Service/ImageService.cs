using AccountAPI.DTO.Response;
using AccountAPI.Service.Interfaces;

namespace AccountAPI.Service
{
    public class ImageService : IImageService
    {
        private readonly HttpClient _httpClient;

        public ImageService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ImageAPI");
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            using var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(file.OpenReadStream());
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
            content.Add(fileContent, "file", file.FileName);

            var response = await _httpClient.PostAsync("/api/images/upload", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Upload ảnh thất bại");
            }

            var result = await response.Content.ReadFromJsonAsync<ImageUploadResponse>();
            return result?.Url ?? throw new Exception("Không nhận được URL ảnh");
        }
    }
}
