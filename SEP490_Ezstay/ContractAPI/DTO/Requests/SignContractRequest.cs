using System.ComponentModel.DataAnnotations;

namespace ContractAPI.DTO.Requests;

public class SignContractRequest
{
    
    [Required]
    public string Signature { get; set; }  
}
