using System.Text.Json;
using ReviewReportAPI.DTO.Response;
using ReviewReportAPI.Service.Interface;

namespace ReviewReportAPI.Service;

public class ReviewClientService : IReviewClientService
{
    private readonly HttpClient _httpClient;

    public ReviewClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ReviewResponse?> GetReviewByIdAsync(Guid reviewId)
    {
        var response = await _httpClient.GetAsync($"/api/Review/{reviewId}");
        if (!response.IsSuccessStatusCode)
            return null;

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ReviewResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }
}
