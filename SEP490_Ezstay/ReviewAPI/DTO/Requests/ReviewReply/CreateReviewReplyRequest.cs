namespace ReviewAPI.DTO.Requests.ReviewReply;

public class CreateReviewReplyRequest
{
    public Guid ReviewId { get; set; }
    public string Content { get; set; }
}