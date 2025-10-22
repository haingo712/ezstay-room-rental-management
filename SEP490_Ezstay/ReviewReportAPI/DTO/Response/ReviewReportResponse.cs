using ReviewReportAPI.Enum;

namespace ReviewReportAPI.DTO.Response;

public class ReviewReportResponse
{
    public Guid Id { get; set; }
    public Guid ReviewId { get; set; }
    public List<string> Images { get; set; }
    public string Reason { get; set; }
    public ReportStatus Status { get; set; }
    public string RejectReason { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime ReviewedAt { get; set; }
}