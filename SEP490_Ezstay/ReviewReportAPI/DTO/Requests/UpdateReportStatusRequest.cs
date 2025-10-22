using System.ComponentModel.DataAnnotations;
using ReviewReportAPI.Enum;

namespace ReviewReportAPI.DTO.Requests;

public class UpdateReportStatusRequest
{
     [Required]
     public ReportStatus Status { get; set; } 
     
     public string? RejectReason { get; set; }
}