namespace Shared.DTOs.BoardingHouse
{
    public class BoardingHouseResponse
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public string HouseName { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public HouseLocationResponse? Location { get; set; }

    }
}
