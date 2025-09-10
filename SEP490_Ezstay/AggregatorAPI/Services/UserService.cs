using AggregatorAPI.Services.Interfaces;

namespace AggregatorAPI.Services;

public class UserService: IUserService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public UserService(IHttpClientFactory httpClientFactory, IConfiguration config)
    {
        _httpClient = httpClientFactory.CreateClient();
        _config = config;
    }

    public async Task<object?> GetUserAsync(Guid userId)
    {
        var gateway = _config["ServiceUrls:Gateway"];
        return await _httpClient.GetFromJsonAsync<object>($"{gateway}/Account/{userId}");
    }
}