using AuthApi.DTO.Response;
using AuthApi.Services.Interfaces;


namespace AuthApi.Services
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
                throw new Exception("Photo upload failed");
            }

            var result = await response.Content.ReadFromJsonAsync<ImageUploadResponse>();
            return result?.Url ?? throw new Exception("Image URL not received");
        }
    }
}