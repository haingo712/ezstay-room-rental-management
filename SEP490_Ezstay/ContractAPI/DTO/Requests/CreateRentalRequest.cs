using System.ComponentModel.DataAnnotations;

namespace ContractAPI.DTO.Requests;

public class CreateRentalRequest
{
    [Required]
    public DateTime CheckinDate { get; set; }
    [Required]
    public DateTime CheckoutDate { get; set; }
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Number of occupants must be at least 1.")]
    public int NumberOfOccupants { get; set; }
}