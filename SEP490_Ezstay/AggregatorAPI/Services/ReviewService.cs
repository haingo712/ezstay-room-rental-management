using AggregatorAPI.Services.Interfaces;

namespace AggregatorAPI.Services;

public class ReviewService: IReviewService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public ReviewService(IHttpClientFactory httpClientFactory, IConfiguration config)
    {
        _httpClient = httpClientFactory.CreateClient();
        _config = config;
    }

    public async Task<object?> GetReviewAsync(Guid reviewId)
    {
        var gateway = _config["ServiceUrls:Gateway"];
        return await _httpClient.GetFromJsonAsync<object>($"{gateway}/Review/{reviewId}");
    }
}