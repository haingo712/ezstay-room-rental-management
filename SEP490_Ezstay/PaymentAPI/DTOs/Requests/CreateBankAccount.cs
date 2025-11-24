using System.ComponentModel.DataAnnotations;

namespace PaymentAPI.DTOs.Requests;

public class CreateBankAccount
{
    [Required]
    public Guid BankGatewayId { get; set; }
    [Required]
    public string AccountNumber { get; set; }
   
    public string? Description { get; set; }
    public bool IsActive { get; set; } 
}