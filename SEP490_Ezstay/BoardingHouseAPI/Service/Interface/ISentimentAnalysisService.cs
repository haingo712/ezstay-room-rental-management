using BoardingHouseAPI.DTO.Response;

namespace BoardingHouseAPI.Service.Interface
{
    public interface ISentimentAnalysisService
    {
        // Sentiment analysis for the given list of reviews
        Task<List<SentimentResponse>> SentimentAnalysisAsync(List<ReviewResponse> reviews);
    }
}
