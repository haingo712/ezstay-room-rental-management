using AccountAPI.Enums;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace AccountAPI.DTO.Request;
    public class CreateUserDTO
    {

        [Required(ErrorMessage = "Gender is required")]
        public GenderEnum Gender { get; set; }
        public IFormFile? Avatar { get; set; }
       
        [StringLength(500, ErrorMessage = "Bio cannot exceed 500 characters")]
        public string? Bio { get; set; }
        [Required(ErrorMessage = "Date of birth is required")]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string DetailAddress { get; set; }
        [Required]
        public string ProvinceId { get; set; }     // Mã tỉnh
        //public string ProvinceName { get; set; }   // Tên tỉnh
        [Required]
        public string WardId { get; set; }         // Mã xã/phường
        //public string WardName { get; set; }       // Tên xã/phường
        [Required]
        public IFormFile FrontImageUrl { get; set; }
        [Required]
        public IFormFile BackImageUrl { get; set; }
        [Required]
        public string TemporaryResidence { get; set; }
        [Required]
        [RegularExpression(@"^\d{12}$", ErrorMessage = "CCCD phải bao gồm đúng 12 chữ số")]
        public string CitizenIdNumber { get; set; }
        [Required]
        public DateTime CitizenIdIssuedDate { get; set; }
        [Required]
        public string CitizenIdIssuedPlace { get; set; }
    }
