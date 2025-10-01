namespace RentalPostsAPI.DTO.Request
{
    public class RentalpostDTO
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public string AuthorName { get; set; } = null!;
        public string RoomName { get; set; } = null!;
        public string HouseName { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string ContactPhone { get; set; } = null!;
        public bool IsActive { get; set; }
        public int? IsApproved { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
