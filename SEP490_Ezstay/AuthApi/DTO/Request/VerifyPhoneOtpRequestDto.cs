using System.ComponentModel.DataAnnotations;

namespace AuthApi.DTO.Request
{
    public class VerifyPhoneOtpRequestDto
    {
        [Required]
        [Phone]
        public string Phone { get; set; }
        [EmailAddress]
        public string Otp { get; set; } 
    }
}
