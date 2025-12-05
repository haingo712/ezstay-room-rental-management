namespace Shared.DTOs.Contracts.Responses;

public class RentalRequestResponse
{
    public Guid Id { get; set; } 
    public Guid UserId { get; set; }  
    public Guid ownerId { get; set; }
    public Guid RoomId { get; set; }  
    
    public DateTime CheckinDate { get; set; }
    public DateTime CheckoutDate { get; set; }
    public int NumberOfOccupants { get; set; }
    public List<string> CitizenIdNumber { get; set; }
    public DateTime CreatedAt { get; set; } 
}