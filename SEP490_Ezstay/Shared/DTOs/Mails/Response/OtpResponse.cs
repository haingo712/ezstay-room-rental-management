namespace Shared.DTOs.Mails.Response;

public class OtpResponse
{
    public Guid Id { get; set; } 
    public string Email { get; set; }
    public string OtpCode { get; set; }
    public DateTime ExpireAt { get; set; }
    public bool IsUsed { get; set; } 
    public Guid ContractId { get; set; }  
}