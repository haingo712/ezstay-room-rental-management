namespace TenantAPI.DTO.Response;

public class RoomResponse
{
    
    public Guid Id { get; set; }
    public string RoomName { get; set; } = null!;
  //  public DateTime UpdatedAt { get; set; }
    public string RoomStatus { get; set; } = string.Empty;
}