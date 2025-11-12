using System.ComponentModel.DataAnnotations;

namespace PaymentAPI.DTOs.Requests;

public class UpdateBankAccount
{
    [Required]
    public string BankName { get; set; }
    [Required]
    public string AccountNumber { get; set; }
  
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}