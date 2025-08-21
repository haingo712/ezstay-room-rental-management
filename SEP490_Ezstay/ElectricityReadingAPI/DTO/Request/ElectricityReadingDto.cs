using System.ComponentModel.DataAnnotations;

namespace ElectricityReadingAPI.DTO.Request;

public class ElectricityReadingDto
{
    public Guid Id { get; set; }
  
    public Guid RoomId { get; set; }
    
    public DateTime CreatedAt { get; set;}
    
    public DateTime ReadingDate  { get; set;}
  
    public decimal OldIndex { get; set;}
    
    public decimal NewIndex { get; set;}
   
    public string Note { get; set; }
    
    public decimal Consumption  {get; set; }
    
    public decimal Total { get; set;}
}