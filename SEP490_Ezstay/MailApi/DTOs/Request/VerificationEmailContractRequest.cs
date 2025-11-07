using System.ComponentModel.DataAnnotations;

namespace MailApi.DTOs.Request;

public class VerificationEmailContractRequest
{
  //  [Required]
  //  public string Email { get; set; } 

    // Otp is required for verification but optional for send requests (backend can generate)
    public string Otp { get; set; }

    // ContractId to associate OTP with a specific contract when sending
   // public Guid? ContractId { get; set; }
    
}
