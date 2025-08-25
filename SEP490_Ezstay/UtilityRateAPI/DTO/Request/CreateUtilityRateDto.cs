using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using UtilityRateAPI.Enum;

namespace UtilityRateAPI.DTO.Request;

public class CreateUtilityRateDto
{
    public Guid OwnerId { get; set; } 
  //  [RegularExpression("^(Electricity|Water)$", ErrorMessage = "Type must be either 'Electricity' or 'Water'.")]
   // public string Type { get; set; } 
   [JsonConverter(typeof(JsonStringEnumConverter))]
   public UtilityType Type { get; set; }
    // [Required]
    // public decimal From { get; set; } 
    [Required]
    public decimal To { get; set; } 
    [Required]
    public decimal Price { get; set; } 
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}