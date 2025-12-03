using Shared.DTOs.Contracts.Responses;

namespace ReviewAPI.DTO.Response;

public class ContractResponse
{
    public Guid Id { get; set; }
    public Guid RoomId { get; set; }
    public DateTime CheckoutDate { get; set; }
    public List<IdentityProfileResponse> IdentityProfiles { get; set; }  
}