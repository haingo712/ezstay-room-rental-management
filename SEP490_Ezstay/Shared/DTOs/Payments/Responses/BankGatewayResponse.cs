namespace Shared.DTOs.Payments.Responses;

public class BankGatewayResponse
{
    public Guid Id { get; set; } 
    public string BankName { get; set; }   
    public string FullName { get; set; } 
    public string Logo { get; set; }
    public bool IsActive { get; set; }
    public DateTime UpdatedAt { get; set; }
}