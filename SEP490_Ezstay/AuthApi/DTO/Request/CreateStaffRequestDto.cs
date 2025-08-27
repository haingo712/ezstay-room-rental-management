using System.ComponentModel.DataAnnotations;

namespace AuthApi.DTO.Request
{
    public class CreateStaffRequestDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string FullName { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Phone { get; set; } = null!;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = null!;
    }
}
