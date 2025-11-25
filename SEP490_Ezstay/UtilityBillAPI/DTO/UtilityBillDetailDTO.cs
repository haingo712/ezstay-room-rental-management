using Shared.DTOs.UtilityReadings.Responses;

namespace UtilityBillAPI.DTO
{
    public class UtilityBillDetailDTO
    {
        public Guid Id { get; set; }
        public Guid UtilityBillId { get; set; }
        public Guid? UtilityReadingId { get; set; }
        public UtilityReadingResponse? UtilityReading { get; set; }
        public string? ServiceName { get; set; }
        public decimal? ServicePrice { get; set; }
        public string Type { get; set; } = null!;       
        public decimal Total { get; set; }
    }
}
