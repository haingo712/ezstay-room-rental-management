namespace RentalPostsAPI.DTO.Request
{
    public class CreateRentalPostDTO
    {
        public Guid RoomId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string ContactPhone { get; set; } = null!;
    }
}
