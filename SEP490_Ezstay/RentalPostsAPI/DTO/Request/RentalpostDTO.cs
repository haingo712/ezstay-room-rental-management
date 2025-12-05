using System.ComponentModel.DataAnnotations;
using Shared.DTOs.BoardingHouse;
using Shared.DTOs.Rooms.Responses;

namespace RentalPostsAPI.DTO.Request
{
    public class RentalpostDTO
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public Guid AuthorId { get; set; }
        public Guid BoardingHouseId { get; set; }
        public List<string>? ImageUrls { get; set; }
        public string AuthorName { get; set; }
        public string RoomName { get; set; } 
        public string HouseName { get; set; }
        public string Title { get; set; } 
        public string Content { get; set; }
        public string ContactPhone { get; set; }
        public bool IsActive { get; set; }
        public int? IsApproved { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public BoardingHouseDTO BoardingHouse { get; set; }
        public RoomDto Room { get; set; } 
      //  public List<ReviewDto>? Reviews { get; set; }
    }
}
