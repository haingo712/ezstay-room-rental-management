namespace Shared.DTOs.UtilityBills.Responses
{
    public class UtilityBillDetailResponse
    {
        public Guid Id { get; set; }
        public Guid UtilityBillId { get; set; }
        public Guid? UtilityReadingId { get; set; }
        public string Type { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public decimal Consumption { get; set; }
        public decimal Total { get; set; }
    }
}
