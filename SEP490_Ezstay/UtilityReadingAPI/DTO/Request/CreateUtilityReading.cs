using System.ComponentModel.DataAnnotations;
using Shared.Enums;
namespace UtilityReadingAPI.DTO.Request;

public class CreateUtilityReading
{
    // [Required]
    // public Guid RoomId { get; set; }
    // [JsonConverter(typeof(JsonStringEnumConverter))]
    public decimal Price { get; set; }
    public UtilityType Type { get; set; }
    
    [StringLength(100, ErrorMessage = "Note cannot exceed 100 characters.")]
    public string? Note { get; set; }
    public decimal CurrentIndex { get; set;}
}