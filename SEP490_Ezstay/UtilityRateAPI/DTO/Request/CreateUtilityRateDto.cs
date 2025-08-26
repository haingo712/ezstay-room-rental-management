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
  //  [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Tier must be a positive integer.")]
    public int To { get; set; } 
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Tier must be a positive integer.")]
    public decimal Price { get; set; } 
  //  public bool IsActive { get; set; }
}