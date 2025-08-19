using System.ComponentModel.DataAnnotations;

namespace BoardingHouseAPI.DTO.Request
{
    public class CreateBoardingHouseDTO
    {
        [Required]
        public Guid OwnerId { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "House name cannot exceed 100 characters.")]
        public string HouseName { get; set; } = null!;
        public string? Description { get; set; }
        
    }
}
