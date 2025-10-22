using System.ComponentModel.DataAnnotations;

namespace ReviewReportAPI.DTO.Requests;

public class UpdateReviewReportRequest
{
    public IFormFileCollection? Images { get; set; }
    [Required]
    [StringLength(1000, ErrorMessage = "Nội dung không được vượt quá 1000 ký tự.")]
    public string Reason { get; set; }
}