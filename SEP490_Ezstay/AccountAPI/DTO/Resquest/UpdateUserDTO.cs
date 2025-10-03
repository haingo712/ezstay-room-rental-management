using AccountAPI.Enums;
using Microsoft.AspNetCore.Http;

namespace AccountAPI.DTO.Request
{
    public class UpdateUserDTO
    {
        public GenderEnum? Gender { get; set; } // dùng ? cho nullable để tránh lỗi FormData
        public string? Bio { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public IFormFile? Avatar { get; set; }

        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? DetailAddress { get; set; }
        public string? ProvinceId { get; set; }
        public string? CommuneId { get; set; }
    }
}
