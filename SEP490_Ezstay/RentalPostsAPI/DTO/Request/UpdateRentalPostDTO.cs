namespace RentalPostsAPI.DTO.Request
{
    public class UpdateRentalPostDTO
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string ContactPhone { get; set; } = null!;
        public List<string>? ImageUrls { get; set; }
        public Guid RoomId { get; set; }
    }
}
