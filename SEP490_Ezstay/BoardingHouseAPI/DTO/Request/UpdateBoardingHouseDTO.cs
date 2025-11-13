using System.ComponentModel.DataAnnotations;

namespace BoardingHouseAPI.DTO.Request
{
    public class UpdateBoardingHouseDTO
    {
        [Required]
        [StringLength(100, ErrorMessage = "House name cannot exceed 100 characters.")]
        public string HouseName { get; set; } = null!;
        public IFormFileCollection? Files { get; set; }
        public string? Description { get; set; }
        public UpdateHouseLocationDTO Location { get; set; } = null!;

    }
}
