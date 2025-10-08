using System.ComponentModel.DataAnnotations;

namespace RoomAmenityAPI.DTO.Request;

public class CreateRoomAmenity
{
   public Guid AmenityId { get; set; }
   // [Required]
   // public string Notes { get; set; }
}