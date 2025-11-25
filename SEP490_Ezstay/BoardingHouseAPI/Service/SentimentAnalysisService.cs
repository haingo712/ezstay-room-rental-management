using BoardingHouseAPI.Service.Interface;
using BoardingHouseAPI.DTO.Response;

namespace BoardingHouseAPI.Service
{
    public class SentimentAnalysisService : ISentimentAnalysisService
    {
        private readonly HttpClient _httpClient;
        public SentimentAnalysisService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<SentimentResponse>> SentimentAnalysisAsync(List<ReviewResponse> reviews)
        {
            if (reviews == null || !reviews.Any())
                return new List<SentimentResponse>();
            var messages = reviews.Select(r => r.Content).ToList();
            var requestBody = new { message = messages };

            var response = await _httpClient.PostAsJsonAsync("/predict-sentiment", requestBody);
            response.EnsureSuccessStatusCode();

            var sentimentResponses = await response.Content.ReadFromJsonAsync<List<SentimentResponse>>();

            return sentimentResponses ?? new List<SentimentResponse>();
        }
    }
}
