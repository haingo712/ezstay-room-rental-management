using System.ComponentModel.DataAnnotations;

namespace ReviewAPI.DTO.Requests;

public class UpdateReviewRequest
{
    [Required]
    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
    public int Rating { get; set; }

    public IFormFileCollection? ImageUrl { get; set; }

    [Required]
    [StringLength(1000, ErrorMessage = "Content must not exceed 1000 characters.")]
    public string Content { get; set; }
}