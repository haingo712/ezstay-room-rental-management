using System.ComponentModel.DataAnnotations;

namespace ReviewAPI.DTO.Requests.ReviewReply;

public class CreateReviewReplyRequest
{
    public IFormFileCollection? Image { get; set; }
    [Required]
    [StringLength(1000, ErrorMessage = "Content must not exceed 1000 characters.")]
    public string Content { get; set; }
}
