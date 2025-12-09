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
            
            // Filter out reviews with null or empty content
            var validMessages = reviews
                .Where(r => !string.IsNullOrWhiteSpace(r.Content))
                .Select(r => r.Content)
                .ToList();
            
            if (!validMessages.Any())
                return new List<SentimentResponse>();
                
            var requestBody = new { message = validMessages };

            try
            {
                var response = await _httpClient.PostAsJsonAsync("/predict-sentiment", requestBody);
                response.EnsureSuccessStatusCode();

                var sentimentResponses = await response.Content.ReadFromJsonAsync<List<SentimentResponse>>();

                return sentimentResponses ?? new List<SentimentResponse>();
            }
            catch (HttpRequestException ex)
            {
                // Log error and return empty list instead of throwing
                Console.WriteLine($"Sentiment Analysis API error: {ex.Message}");
                return new List<SentimentResponse>();
            }
        }
    }
}
