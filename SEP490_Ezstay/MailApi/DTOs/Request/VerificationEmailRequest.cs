namespace MailApi.DTOs.Request
{
    public class VerificationEmailRequest
    {
        public string Email { get; set; } = null!;
        public string Otp { get; set; } = null!;
    }
}
