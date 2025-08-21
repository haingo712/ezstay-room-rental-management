using System.ComponentModel.DataAnnotations;

namespace UtilityReadingAPI.DTO.Request;

public class UpdateUtilityReadingDto
{
    [Required]
    public Guid RoomId { get; set; }
    [Required]
    [RegularExpression("^(Electricity|Water)$", ErrorMessage = "Type must be either 'Electricity' or 'Water'.")]
    public string Type { get; set; }
    public DateTime CreatedAt { get; set;}
    [Required]
    public DateTime ReadingDate  { get; set;}
    
    [StringLength(100, ErrorMessage = "Note cannot exceed 100 characters.")]
    public string Note { get; set; }
   
    [Required]
    public decimal PreviousIndex { get; set;}
    public decimal CurrentIndex { get; set;}
}