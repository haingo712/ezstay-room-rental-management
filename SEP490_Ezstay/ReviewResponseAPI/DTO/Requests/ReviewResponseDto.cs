namespace ReviewResponseAPI.DTO.Requests;

public class ReviewResponseDto
{
    public Guid Id { get; set; } 
    public Guid OwnerId { get; set; } 
    public Guid ReviewId { get; set; }
    public string ResponeContent { get; set; }
    public DateTime CreatedAt { get; set; } 
    public DateTime UpdatedAt { get; set; }
}