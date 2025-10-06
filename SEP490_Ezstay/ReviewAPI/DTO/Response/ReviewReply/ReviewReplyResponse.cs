namespace ReviewAPI.DTO.Response.ReviewReply;

public class ReviewReplyResponse
{
    public Guid Id { get; set; }
    public Guid ReviewId { get; set; }
    public Guid OwnerId { get; set; }
    public string Image { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}