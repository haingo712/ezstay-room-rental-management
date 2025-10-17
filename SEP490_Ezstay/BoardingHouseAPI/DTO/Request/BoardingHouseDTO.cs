namespace BoardingHouseAPI.DTO.Request
{
    public class BoardingHouseDTO
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public string HouseName { get; set; } = null!;
        public List<string>? ImageUrls { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public HouseLocationDTO? Location { get; set; }

    }
}
