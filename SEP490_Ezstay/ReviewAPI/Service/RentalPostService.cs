using System.Text.Json;
using ReviewAPI.DTO.Response;
using ReviewAPI.Service.Interface;
using Shared.DTOs;

namespace ReviewAPI.Service;

public class RentalPostService : IRentalPostService
{
    private readonly HttpClient _httpClient;

    public RentalPostService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<Guid?> GetPostIdByRoomIdAsync(Guid roomId)
    {
        var response = await _httpClient.GetAsync($"/api/RentalPosts/RoomId/{roomId}");
        if (!response.IsSuccessStatusCode) return null;

        var postId = await response.Content.ReadFromJsonAsync<Guid>();
        return postId;
    }

    public async Task<PostResponse?> GetByIdAsync(Guid postId)
    {
        var response = await _httpClient.GetAsync($"/api/RentalPosts/{postId}");
        if (!response.IsSuccessStatusCode)
            return null;
        var wrapper = await response.Content.ReadFromJsonAsync<ApiResponse<PostResponse>>(
            new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        return wrapper?.Data;
        // var raw = await response.Content.ReadAsStringAsync();
        // Console.WriteLine("RAW JSON from RentalPostsAPI: " + raw);
        //
        // return JsonSerializer.Deserialize<PostResponse>(
        //     raw,
        //     new JsonSerializerOptions
        //     {
        //         PropertyNameCaseInsensitive = true
        //     }
        // );
    }
}