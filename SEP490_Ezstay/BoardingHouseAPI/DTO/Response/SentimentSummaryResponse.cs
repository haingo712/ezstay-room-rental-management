namespace BoardingHouseAPI.DTO.Response
{
    public class SentimentSummaryResponse
    {
        public Guid BoardingHouseId { get; set; }
        public int TotalReviews { get; set; }
        public int PositiveCount { get; set; }
        public int NeutralCount { get; set; }
        public int NegativeCount { get; set; }
        public List<SentimentResponse>? Details { get; set; }
    }
}
