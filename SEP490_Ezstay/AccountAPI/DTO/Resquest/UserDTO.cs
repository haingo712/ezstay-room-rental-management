using AccountAPI.Enums;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace AccountAPI.DTO.Request
{
    public class UserDTO
    {

        [Required(ErrorMessage = "Gender is required")]
        public GenderEnum Gender { get; set; }
        
        [Required(ErrorMessage = "Avatar is required")]
        public IFormFile Avatar { get; set; }
        
        [Required(ErrorMessage = "Bio is required")]
        [StringLength(500, ErrorMessage = "Bio cannot exceed 500 characters")]
        public string Bio { get; set; }
        
        [Required(ErrorMessage = "Date of birth is required")]
        public DateTime DateOfBirth { get; set; }
      
        public string? DetailAddress { get; set; }
        public string? ProvinceId { get; set; }     // Mã tỉnh
        //public string ProvinceName { get; set; }   // Tên tỉnh
        public string? WardId { get; set; }         // Mã xã/phường
        //public string WardName { get; set; }       // Tên xã/phường

        public IFormFile? FrontImageUrl { get; set; }
        public IFormFile? BackImageUrl { get; set; }
        public string? TemporaryResidence { get; set; }
        public string? CitizenIdNumber { get; set; }
        public DateTime? CitizenIdIssuedDate { get; set; }
        public string? CitizenIdIssuedPlace { get; set; }
    }
}
