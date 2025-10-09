namespace RentalPostsAPI.DTO.Request
{
    public class RentalpostDTO
    {
        public Guid Id { get; set; }
        public List<Guid>? RoomId { get; set; }
        public Guid AuthorId { get; set; }
        public Guid BoardingHouseId { get; set; }
        public List<string>? ImageUrls { get; set; }
        public string AuthorName { get; set; } = null!;
        public string RoomName { get; set; } = null!;
        public string HouseName { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string ContactPhone { get; set; } = null!;
        public bool IsActive { get; set; }
        public int? IsApproved { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
