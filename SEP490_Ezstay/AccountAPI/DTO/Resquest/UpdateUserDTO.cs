using AccountAPI.Enums;
using Microsoft.AspNetCore.Http;

namespace AccountAPI.DTO.Request
{
    public class UpdateUserDTO
    {
        public GenderEnum? Gender { get; set; }
        public string? Bio { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public IFormFile? Avatar { get; set; }

        public string? FullName { get; set; }
        public string? Phone { get; set; }

        public string? ProvinceCode { get; set; }
        public string? CommuneCode { get; set; }

    }
}
