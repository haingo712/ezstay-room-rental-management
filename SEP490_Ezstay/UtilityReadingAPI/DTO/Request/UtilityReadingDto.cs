using System.Text.Json.Serialization;
using UtilityReadingAPI.Enum;

namespace UtilityReadingAPI.DTO.Request;

public class UtilityReadingDto
{
    
  
    public Guid Id { get; set; }
  
    public Guid RoomId { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UtilityType Type { get; set; }
    
    public DateTime ReadingDate  { get; set;}
    public DateTime UpdatedAt  { get; set;}
  
    public decimal PreviousIndex { get; set;}
  
    public decimal CurrentIndex { get; set;}
   
    public string Note { get; set; }
    
    public decimal Consumption  {get; set; }
    
    public decimal Total { get; set;}
}