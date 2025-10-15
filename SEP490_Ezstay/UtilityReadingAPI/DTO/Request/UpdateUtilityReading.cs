using System.ComponentModel.DataAnnotations;

namespace UtilityReadingAPI.DTO.Request;

public class UpdateUtilityReading
{
    
    public decimal Price { get; set; }
    [StringLength(100, ErrorMessage = "Note cannot exceed 100 characters.")]
    public string? Note { get; set; }
    [Required]
    public decimal CurrentIndex { get; set;}
}