using System.ComponentModel.DataAnnotations;

namespace HouseLocationAPI.DTO.Request
{
    public class UpdateHouseLocationDTO
    {
        [Required]
        public Guid HouseId { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "Address cannot exceed 500 characters.")]
        public string FullAddress { get; set; } = null!;

    }
}
