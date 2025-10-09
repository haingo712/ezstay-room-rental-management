using AccountAPI.Enums;
using Microsoft.AspNetCore.Http;

namespace AccountAPI.DTO.Request
{
    public class UpdateUserDTO
    {
        public GenderEnum? Gender { get; set; }
        public IFormFile? Avatar { get; set; }
        public string? Bio { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public string? DetailAddress { get; set; }
        public string? ProvinceId { get; set; }     // Mã tỉnh
        //public string ProvinceName { get; set; }   // Tên tỉnh
        public string? WardId { get; set; }         // Mã xã/phường
                                                   //public string WardName { get; set; }       // Tên xã/phường
        public string? FullName { get; set; }
        public IFormFile? FrontImageUrl { get; set; }
        public IFormFile? BackImageUrl { get; set; }
        public string? TemporaryResidence { get; set; }
        public string? CitizenIdNumber { get; set; }
        public DateTime? CitizenIdIssuedDate { get; set; }
        public string? CitizenIdIssuedPlace { get; set; }
    }
}
