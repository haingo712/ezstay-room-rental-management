using System.ComponentModel.DataAnnotations;

namespace ContractAPI.DTO.Requests;

public class UpdateIdentityProfile
{
    [Required]
    public Guid UserId { get; set; } 
    [Required]
    public string Gender { get; set; }
    [Required]
    public string Avatar { get; set; } 
    [Required]
    public string FullName { get; set; }
    [Required]
    public DateTime DateOfBirth { get; set; }
    [Required]
    [Phone]
    public string Phone { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string ProvinceId { get; set; }     
    [Required]
    public string ProvinceName { get; set; }  
    [Required]
    public string WardId { get; set; }       
    [Required]
    public string WardName { get; set; }      
    [Required]
    public string Address { get; set; }
    [Required]
    public string TemporaryResidence { get; set; }
    [Required]
    public string CitizenIdNumber { get; set; }
    [Required]
    public DateTime CitizenIdIssuedDate { get; set; }
    [Required]
    public string CitizenIdIssuedPlace { get; set; }
    [Required]
    public string FrontImageUrl { get; set; }
    [Required]
    public string BackImageUrl { get; set; }
}