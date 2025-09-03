using Microsoft.Build.Framework;
using RentalRequestAPI.Enum;

namespace RentalRequestAPI.DTO.Request;

public class UpdateRentalRequestByOwnerDto
{
    // Chủ nhà (owner) duyệt đơn
  
        [Required]
        public RequestStatus Status { get; set; } 
        public string? OwnerNotes { get; set; }

}