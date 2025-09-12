using System.ComponentModel.DataAnnotations;

namespace BoardingHouseAPI.DTO.Request
{
    public class CreateHouseLocationDTO
    {
        [Required]
        public string ProvinceId { get; set; } = null!;

        [Required]
        public string CommuneId { get; set; } = null!;

        [Required]
        [StringLength(300, ErrorMessage = "Address detail cannot exceed 300 characters.")]
        public string AddressDetail { get; set; } = null!;
    }
}
