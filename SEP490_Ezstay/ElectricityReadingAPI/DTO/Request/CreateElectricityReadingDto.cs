using System.ComponentModel.DataAnnotations;

namespace ElectricityReadingAPI.DTO.Request;

public class CreateElectricityReadingDto
{
    [Required]
    public Guid RoomId { get; set; }
    public DateTime CreatedAt { get; set;}
    public DateTime ReadingDate  { get; set;}
    [Required]
    public decimal OldIndex { get; set;}
    public decimal NewIndex { get; set;}
    [StringLength(100, ErrorMessage = "Note cannot exceed 100 characters.")]
    public string Note { get; set; }
    public decimal Consumption { get; set;}
    public decimal Total { get; set;}
    
}