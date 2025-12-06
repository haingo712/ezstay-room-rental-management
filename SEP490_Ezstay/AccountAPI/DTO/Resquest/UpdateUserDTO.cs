using System.ComponentModel.DataAnnotations;
using AccountAPI.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AccountAPI.DTO.Request
{
    public class UpdateUserDTO
    {
        [Required(ErrorMessage = "Gender is required")]
        public GenderEnum Gender { get; set; }
        
        public string? Avatar { get; set; }
        
        [Required(ErrorMessage = "Bio is required")]
        [StringLength(500, ErrorMessage = "Bio cannot exceed 500 characters")]
        public string Bio { get; set; }
        
        [Required(ErrorMessage = "Date of birth is required")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Detail address is required")]
        public string DetailAddress { get; set; }
        
        [Required(ErrorMessage = "Province is required")]
        public string ProvinceId { get; set; }     // Mã tỉnh

        public string? WardId { get; set; }         // Mã xã/phường
                                                    //public string WardName { get; set; }       // Tên xã/phường
        public string? FullName { get; set; }
        public string? FrontImageUrl { get; set; }
        public string? BackImageUrl { get; set; }
        public string? TemporaryResidence { get; set; }
        [Required(ErrorMessage = "CitizenIdNumber is required")]
        [RegularExpression(@"^\d{12}$", ErrorMessage = "CitizenIdNumber must be exactly 12 digits and numeric only")]
        public string? CitizenIdNumber { get; set; }
        public DateTime? CitizenIdIssuedDate { get; set; }
        public string? CitizenIdIssuedPlace { get; set; }
    }
}
