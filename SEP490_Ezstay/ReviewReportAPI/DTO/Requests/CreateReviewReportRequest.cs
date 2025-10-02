using System.ComponentModel.DataAnnotations;

namespace ReviewReportAPI.DTO.Requests;

public class CreateReviewReportRequest
{

    [Required]
    [StringLength(1000, ErrorMessage = "Nội dung không được vượt quá 1000 ký tự.")]
    public string Reason { get; set; }
    public Guid ImageId { get; set; }
}    
