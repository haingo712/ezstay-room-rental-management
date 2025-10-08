namespace IdentityProfileAPI.DTO.Response;

public class CreateIdentityProfileResponse
{
    public string FullName { get; set; }              
    public DateTime DateOfBirth { get; set; }         
    public string PhoneNumber { get; set; }
    public string Email { get; set; } 
    public string ProvinceId { get; set; }     
    public string WardId { get; set; }        
    public string Address { get; set; }
    public string TemporaryResidence { get; set; }     
    public string CitizenIdNumber { get; set; }       
    public DateTime CitizenIdIssuedDate { get; set; }  
    public string CitizenIdIssuedPlace { get; set; }  
    public string Notes { get; set; }                  
    public string AvatarUrl { get; set; }
    public string FrontImageUrl { get; set; }
    public string BackImageUrl { get; set; }
}