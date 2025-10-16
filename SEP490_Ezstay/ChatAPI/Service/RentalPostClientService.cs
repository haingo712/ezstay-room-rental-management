using System.Text.Json;
using ChatAPI.Service.Interface;
using Shared.DTOs;
using Shared.DTOs.RentalPosts.Responses;

namespace ChatAPI.Service;

public class RentalPostClientService: IRentalPostClientService
{
    private readonly HttpClient _httpClient;

    public RentalPostClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<RentalPostResponse?> GetById(Guid postId)
    {
            var response = await _httpClient.GetAsync($"api/RentalPosts/{postId}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<RentalPostResponse>>();
            return apiResponse?.Data;
    }
    
}