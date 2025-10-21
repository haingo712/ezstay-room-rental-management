namespace BoardingHouseAPI.DTO.Response;

public class ReviewResponse
{
    public Guid Id { get; set; }    
    public int Rating { get; set; }
    public string Content { get; set; }    
}