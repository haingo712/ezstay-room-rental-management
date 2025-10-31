namespace RentalPostsAPI.DTO.Request
{
    public class CreateRentalPostDTO
    {
        public Guid BoardingHouseId { get; set; }
        public List<Guid>? RoomId { get; set; }
        public bool IsAllRooms { get; set; } = false;
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string ContactPhone { get; set; } = null!;
        public List<IFormFile>? Images { get; set; }

    }
}
