using System.Text.Json;
using BoardingHouseAPI.DTO.Response;
using BoardingHouseAPI.Service.Interface;

namespace BoardingHouseAPI.Service
{
    public class ReviewService : IReviewService
    {
        private readonly HttpClient _httpClient;
        public ReviewService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // get all reviews by list of rental posts
        public async Task<List<ReviewResponse>?> GetReviewsByRoomsAsync(List<RoomResponse> rooms)
        {
            if (rooms == null || !rooms.Any())
                return new List<ReviewResponse>();

            var allReviews = new List<ReviewResponse>();

            foreach (var room in rooms)
            {
                // Use the correct endpoint: GET /api/Review/all/{roomId}
                var response = await _httpClient.GetAsync($"api/Review/all/{room.Id}");

                if (!response.IsSuccessStatusCode)
                    continue;

                using var stream = await response.Content.ReadAsStreamAsync();                

                var reviews = await JsonSerializer.DeserializeAsync<List<ReviewResponse>>(
                stream,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (reviews != null && reviews.Any())
                    allReviews.AddRange(reviews);
            }

            return allReviews;
        }
    }
}
