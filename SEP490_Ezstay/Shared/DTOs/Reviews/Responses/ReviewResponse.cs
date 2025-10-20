namespace Shared.DTOs.Reviews.Responses;

public class ReviewResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ContractId { get; set; }
    // public Guid PostId { get; set; }
    public Guid RoomId { get; set; }
    public string ImageUrl { get; set; }
    public DateTime ReviewDeadline { get; set; }
    public int Rating { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime DeletedAt { get; set; }
}