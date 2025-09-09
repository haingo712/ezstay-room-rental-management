namespace RentalPostsAPI.DTO.Request
{
    public class RentalpostDTO
    {
        public Guid Id { get; set; }
        public Guid AuthorId { get; set; }
        public Guid RoomId { get; set; }

        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string ContactPhone { get; set; } = null!;

        public bool IsActive { get; set; }
        public int? IsApproved { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
