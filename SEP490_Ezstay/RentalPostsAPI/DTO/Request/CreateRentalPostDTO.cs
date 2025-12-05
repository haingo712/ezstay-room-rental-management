using System.ComponentModel.DataAnnotations;

namespace RentalPostsAPI.DTO.Request
{
    public class CreateRentalPostDTO
    {
        [Required]
        public Guid BoardingHouseId { get; set; }
        [Required]
        public Guid RoomId { get; set; }
        // public bool IsAllRooms { get; set; } = false;
        [Required]
        public string Title { get; set; } 
        [Required]
        public string Content { get; set; }
        public List<IFormFile>? Images { get; set; }

    }
}
