
using System.ComponentModel.DataAnnotations;
using Shared.Enums;

namespace AuthApi.DTO.Request
{
    public class AccountRequest
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone must be exactly 10 digits")]
        public string Phone { get; set; }
        public RoleEnum Role { get; set; } 
    }
}
