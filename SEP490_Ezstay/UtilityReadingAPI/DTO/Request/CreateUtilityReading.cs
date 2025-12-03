using System.ComponentModel.DataAnnotations;

namespace UtilityReadingAPI.DTO.Request;

public class CreateUtilityReading
{
    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0.")]
    public decimal Price { get; set; }
    [StringLength(100, ErrorMessage = "Note cannot exceed 100 characters.")]
    public string? Note { get; set; }
    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Current Index must be greater than or equal to 0.")]
    public decimal CurrentIndex { get; set;}
}