
using Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace AuthApi.DTO.Request
{
    public class AccountRequest
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone must be exactly 10 digits")]
        public string Phone { get; set; } = null!;
        public RoleEnum Role { get; set; } 
    }
}
