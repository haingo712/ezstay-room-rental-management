using System.ComponentModel.DataAnnotations;

namespace AuthApi.DTO.Request
{
    public class RegisterRequestDto
    {
        public string FullName { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        [Phone]
        public string Phone { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}
