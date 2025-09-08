using System.ComponentModel.DataAnnotations;

namespace HouseLocationAPI.DTO.Request
{
    public class UpdateHouseLocationDTO
    {
        [Required]
        public Guid HouseId { get; set; }
        [Required]
        public string ProvinceId { get; set; } = null!;      
        [Required]
        public string CommuneId { get; set; } = null!;     
        [Required]
        [StringLength(300, ErrorMessage = "Address detail cannot exceed 300 characters.")]
        public string AddressDetail { get; set; } = null!;        
    }
}
