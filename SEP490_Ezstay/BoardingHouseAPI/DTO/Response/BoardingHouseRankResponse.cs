using BoardingHouseAPI.Enum;

namespace BoardingHouseAPI.DTO.Response
{
    public class BoardingHouseRankResponse
    {
        public Guid BoardingHouseId { get; set; }
        public string HouseName { get; set; } = null!;
        public string FullAddress { get; set; } = null!;
        public double Score { get; set; }
        public RankType Type { get; set; }
    }
}
