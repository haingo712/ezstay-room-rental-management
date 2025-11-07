using System.ComponentModel.DataAnnotations;
using ContractAPI.DTO.Requests.UtilityReading;


namespace ContractAPI.DTO.Requests;

public class CreateContract
{
    [Required]
    public Guid RoomId { get; set; }
    [Required]
    public DateTime CheckinDate { get; set; }
    [Required]
    public DateTime CheckoutDate { get; set; }
    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Tiền cọc phải >= 0")]
    public decimal DepositAmount { get; set; }
    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Tiền phòng  phải >= 0")]
    public decimal RoomPrice { get; set;}
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Number of occupants must be at least 1.")]
    public int NumberOfOccupants { get; set; }
    [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
    public string? Notes { get; set; }
    [Required]
    public List<CreateIdentityProfile> ProfilesInContract { get; set; } 
    [Required]
    public CreateUtilityReadingContract ElectricityReading { get; set; }
    [Required]
    public CreateUtilityReadingContract WaterReading { get; set; }
}