namespace BoardingHouseAPI.DTO.Response
{
    public class SentimentResponse
    {
        public string Message { get; set; } = null!;
        public string Label { get; set; } = null!;
        public double Confidence { get; set; }
    }
}
