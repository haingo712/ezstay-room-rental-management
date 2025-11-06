using System.ComponentModel.DataAnnotations;

namespace MailApi.DTOs.Request;

public class VerificationEmailContractRequest
{
    [Required]
    public string Email { get; set; } 
    [Required]
    public string Otp { get; set; } 
    public Guid? ContractId { get; set; }  
}
