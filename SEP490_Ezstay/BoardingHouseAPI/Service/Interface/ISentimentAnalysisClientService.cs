using BoardingHouseAPI.DTO.Response;

namespace BoardingHouseAPI.Service.Interface
{
    public interface ISentimentAnalysisClientService
    {
        // Sentiment analysis for the given list of reviews
        Task<List<SentimentResponse>> SentimentAnalysisAsync(List<ReviewResponse> reviews);
    }
}
