namespace Shared.DTOs.Contracts.Responses;

public class IdentityProfileResponse
{ 
   public Guid Id { get; set; }
   public Guid UserId { get; set; }
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
   public bool  IsSigner{ get; set; }
    
}

