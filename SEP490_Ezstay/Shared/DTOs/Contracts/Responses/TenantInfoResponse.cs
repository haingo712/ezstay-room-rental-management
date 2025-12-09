namespace Shared.DTOs.Contracts.Responses;

public class TenantInfoResponse
{
    public Guid Id { get; set; }
    public Guid ContractId { get; set; }
    public Guid UserId { get; set; }
    public bool IsPrimary { get; set; } // First tenant in ProfilesInContract
    
    // Contract Info
    public Guid RoomId { get; set; }
    public string RoomName { get; set; }
    public string HouseName { get; set; }
    public DateTime CheckinDate { get; set; }
    public DateTime CheckoutDate { get; set; }
    
    // Identity Profile Info
    public string FullName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Gender { get; set; }
    public string ProvinceId { get; set; }   
    public string ProvinceName { get; set; }   
    public string WardId { get; set; }       
    public string WardName { get; set; }      
    public string Address { get; set; }
    public string TemporaryResidence { get; set; }
    public string CitizenIdNumber { get; set; }
    public DateTime CitizenIdIssuedDate { get; set; }
    public string CitizenIdIssuedPlace { get; set; }
    public string FrontImageUrl { get; set; }
    public string BackImageUrl { get; set; }
}
