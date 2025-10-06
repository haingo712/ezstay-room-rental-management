using System.ComponentModel.DataAnnotations;

namespace ReviewAPI.DTO.Requests.ReviewReply;

public class CreateReviewReplyRequest
{
    public IFormFile? Image { get; set; }
    [Required]
    [StringLength(1000, ErrorMessage = "Nội dung không được vượt quá 1000 ký tự.")]
    public string Content { get; set; }
}