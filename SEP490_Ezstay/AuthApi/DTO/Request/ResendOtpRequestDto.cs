using System.ComponentModel.DataAnnotations;

namespace AuthApi.DTO.Request
{
    public class ResendOtpRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
