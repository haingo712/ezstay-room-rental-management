using System.ComponentModel.DataAnnotations;

namespace UtilityReadingAPI.DTO.Request;

public class UtilityReadingDto
{
    
  
    public Guid Id { get; set; }
  
    public Guid RoomId { get; set; }
    public string Type { get; set; }
    
    public DateTime ReadingDate  { get; set;}
  
    public decimal PreviousIndex { get; set;}
  
    public decimal CurrentIndex { get; set;}
   
    public string Note { get; set; }
    
    public decimal Consumption  {get; set; }
    
    public decimal Total { get; set;}
}