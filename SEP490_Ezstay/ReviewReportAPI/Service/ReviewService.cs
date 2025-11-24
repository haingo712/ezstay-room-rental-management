using System.Text.Json;
using ReviewReportAPI.DTO.Response;
using ReviewReportAPI.Service.Interface;
using Shared.DTOs.Reviews.Responses;
namespace ReviewReportAPI.Service;

public class ReviewService : IReviewService
{
    private readonly HttpClient _httpClient;

    public ReviewService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
   
    // public async Task<ReviewResponse?> GetReviewById(Guid reviewId)
    // {
    //     var response = await _httpClient.GetAsync($"/api/Review/{reviewId}");
    //     if (!response.IsSuccessStatusCode)
    //         return null;
    //
    //     var json = await response.Content.ReadAsStringAsync();
    //     return JsonSerializer.Deserialize<ReviewResponse>(json, new JsonSerializerOptions
    //     {
    //         PropertyNameCaseInsensitive = true
    //     });
    // }
    public async Task<bool> HideReview(Guid reviewId, bool hide)
    {
        var response = await _httpClient.PutAsync($"/api/Review/{reviewId}/hide/{hide}", null);
        return response.IsSuccessStatusCode;
        // var json = await response.Content.ReadAsStringAsync();
        // return JsonSerializer.Deserialize<bool>(json, new JsonSerializerOptions
        // {
        //     PropertyNameCaseInsensitive = true
        // });
    }
}
