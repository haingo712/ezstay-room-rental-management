using System.ComponentModel.DataAnnotations;

namespace AuthApi.DTO.Request
{
    public class RegisterRequestDto
    {
        public string FullName { get; set; } = null!;
        [EmailAddress(ErrorMessage = "Email is not valid")]
        public string Email { get; set; } = null!;
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone must be exactly 10 digits")]
        public string Phone { get; set; } = null!;
        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\W).+$",
     ErrorMessage = "Password must contain at least one uppercase letter and one special character.")]
        public string Password { get; set; } = null!;
    }
}
