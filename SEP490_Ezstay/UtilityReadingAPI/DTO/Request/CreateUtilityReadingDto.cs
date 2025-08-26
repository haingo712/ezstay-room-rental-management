using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using UtilityReadingAPI.Enum;

namespace UtilityReadingAPI.DTO.Request;

public class CreateUtilityReadingDto
{
    [Required]
    public Guid RoomId { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UtilityType Type { get; set; }
    
    [StringLength(100, ErrorMessage = "Note cannot exceed 100 characters.")]
    public string Note { get; set; }
    [Required]
    public decimal PreviousIndex { get; set;}
    public decimal CurrentIndex { get; set;}
}