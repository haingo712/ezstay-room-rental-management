using System.ComponentModel.DataAnnotations;

namespace ReviewResponseAPI.DTO.Requests;

public class CreateReviewResponseDto
{
    [Required]
    [StringLength(1000, ErrorMessage = "Nội dung không được vượt quá 1000 ký tự.")]
    public string ResponeContent { get; set; }
}
