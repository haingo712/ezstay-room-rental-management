using System.ComponentModel.DataAnnotations;


namespace IdentityProfileAPI.DTO.Requests;

public class CreateIdentityProfileDto
{
    [Required]
    public string FullName { get; set; }
    [Required]
    public DateTime DateOfBirth { get; set; }
    [Required]
    [Phone]
    public string PhoneNumber { get; set; }
    [EmailAddress]
    public string? Email { get; set; }
    
    [Required]
    public string ProvinceId { get; set; }     // Mã tỉnh
    [Required]
    public string WardId { get; set; }         // Mã xã/phường
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
    public string? Notes { get; set; }
    public string? AvatarUrl { get; set; }
    [Required]
    public string FrontImageUrl { get; set; }
    [Required]
    public string BackImageUrl { get; set; }
  
}