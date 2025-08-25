using System.ComponentModel.DataAnnotations;
using UtilityRateAPI.Enum;
using System.Text.Json.Serialization;
namespace UtilityRateAPI.DTO.Request;

public class UtilityRateDto
{
    
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; } 
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UtilityType Type { get; set; }
    public int Tier { get; set; }
    public decimal From { get; set; } 
    public decimal To { get; set; } 
    public decimal Price { get; set; } 
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}