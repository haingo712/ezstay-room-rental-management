using Microsoft.Build.Framework;

namespace TenantAPI.DTO.Requests;

public class CreateIdentityProfileDto
{
    [Required]
    public string FullName { get; set; }
    public DateTime DateOfBirth { get; set; }
    [Required]
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Province { get; set; }
    public string District { get; set; }
    public string Ward { get; set; }
    public string Address { get; set; }
    public string TemporaryResidence { get; set; }
    [Required]
    public string CitizenIdNumber { get; set; }
    [Required]
    public DateTime CitizenIdIssuedDate { get; set; }
    [Required]
    public string CitizenIdIssuedPlace { get; set; }
    public string? Notes { get; set; }
    public string AvatarUrl { get; set; }
    [Required]
    public string FrontImageUrl { get; set; }
    [Required]
    public string BackImageUrl { get; set; }
}