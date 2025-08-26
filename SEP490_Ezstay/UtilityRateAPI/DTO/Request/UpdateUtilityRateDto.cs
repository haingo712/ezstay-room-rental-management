using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using UtilityRateAPI.Enum;

namespace UtilityRateAPI.DTO.Request;

public class UpdateUtilityRateDto
{
  //[JsonConverter(typeof(JsonStringEnumConverter))]
  // public UtilityType Type { get; set; }
 //   public int Tier { get; set; }
    [Required]
    public int To { get; set; } 
    [Required]
    public decimal Price { get; set; } 
  //  public bool IsActive { get; set; }
}