using System.ComponentModel.DataAnnotations;

namespace ReviewAPI.DTO.Requests;

public class UpdateReviewRequest
{
    [Required]
    [Range(1, 5, ErrorMessage = "Rating phải nằm trong khoảng 1 đến 5.")]
    public int Rating { get; set; }
    [Required]
    public IFormFileCollection ImageUrl { get; set; }
    [Required]
    [StringLength(1000, ErrorMessage = "Nội dung không được vượt quá 1000 ký tự.")]
    public string Content { get; set; }
}