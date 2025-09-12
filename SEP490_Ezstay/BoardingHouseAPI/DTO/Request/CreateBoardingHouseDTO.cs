using System.ComponentModel.DataAnnotations;

namespace BoardingHouseAPI.DTO.Request
{
    public class CreateBoardingHouseDTO
    {      
        [Required]
        [StringLength(100, ErrorMessage = "House name cannot exceed 100 characters.")]
        public string HouseName { get; set; } = null!;
        public string? Description { get; set; }
        [Required]
        public CreateHouseLocationDTO Location { get; set; } = null!;

    }
}
