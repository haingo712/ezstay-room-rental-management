
using System.Text.Json.Serialization;
using Shared.Enums;

namespace Shared.DTOs.UtilityReadings.Responses;

public class UtilityReadingResponse
{
    public Guid Id { get; set; }
    // public Guid RoomId { get; set; }
    public Guid ContractId  { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UtilityType Type { get; set; }
    public decimal Price { get; set; }
    public DateTime ReadingDate  { get; set;}
    public DateTime UpdatedAt  { get; set;}
  
    public decimal PreviousIndex { get; set;}
  
    public decimal CurrentIndex { get; set;}
   
    public string Note { get; set; }
    
    public decimal Consumption  {get; set; }
    
    public decimal Total { get; set;}
}