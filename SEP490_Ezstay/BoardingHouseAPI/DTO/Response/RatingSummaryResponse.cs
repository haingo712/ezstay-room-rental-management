namespace BoardingHouseAPI.DTO.Response
{
    public class RatingSummaryResponse
    {
        public Guid BoardingHouseId { get; set; }
        public int TotalReviews { get; set; }
        public double AverageRating { get; set; }
        public int OneStarCount { get; set; }
        public int TwoStarCount { get; set; }
        public int ThreeStarCount { get; set; }
        public int FourStarCount { get; set; }
        public int FiveStarCount { get; set; }
        public List<ReviewResponse>? Details { get; set; }
    }
}
