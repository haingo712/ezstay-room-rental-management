namespace ContractAPI.DTO.Requests;

public class CreateContractWithProfileDto
{
    public CreateContractDto Contract { get; set; }
    public CreateIdentityProfileDto IdentityProfiles { get; set; } 
}