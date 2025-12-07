using System.ComponentModel.DataAnnotations;
using AccountAPI.Enums;
using Microsoft.AspNetCore.Http;

namespace AccountAPI.DTO.Request
{
    public class UpdateUserDTO
    {
        [Required(ErrorMessage = "Gender is required")]
        public GenderEnum Gender { get; set; }
        public string? Avatar { get; set; }
        
        [StringLength(500, ErrorMessage = "Bio cannot exceed 500 characters")]
        public string? Bio { get; set; }
        [Phone]
        [Required(ErrorMessage = "Phone number is required")]
        public string Phone { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Date of birth is required")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Detail address is required")]
        public string DetailAddress { get; set; }
        
        [Required(ErrorMessage = "Province is required")]
        public string ProvinceId { get; set; }     // Mã tỉnh

        [Required(ErrorMessage = "Ward is required")]
        public string WardId { get; set; }         // Mã xã/phường
                                                    
        [Required(ErrorMessage = "Full name is required")]
        public string FullName { get; set; }
        
        [Required(ErrorMessage = "Front image of citizen ID is required")]
        public string FrontImageUrl { get; set; }
        
        [Required(ErrorMessage = "Back image of citizen ID is required")]
        public string BackImageUrl { get; set; }
        
        [Required(ErrorMessage = "Temporary residence is required")]
        public string TemporaryResidence { get; set; }
        
        [Required(ErrorMessage = "Citizen ID number is required")]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "Citizen ID must be exactly 12 digits")]
        [RegularExpression(@"^\d{12}$", ErrorMessage = "Citizen ID must contain only digits")]
        public string CitizenIdNumber { get; set; }
        
        [Required(ErrorMessage = "Citizen ID issued date is required")]
        public DateTime CitizenIdIssuedDate { get; set; }
        
        [Required(ErrorMessage = "Citizen ID issued place is required")]
        public string CitizenIdIssuedPlace { get; set; }
    }
}
