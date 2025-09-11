using System.ComponentModel.DataAnnotations;

namespace RoomAmenityAPI.DTO.Request;

public class CreateRoomAmenityDto
{
   public Guid AmenityId { get; set; }
   [Required]
   public string Notes { get; set; }
}