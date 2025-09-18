namespace AuthApi.DTO.Request
{
    public class VerifyPhoneOtpRequestDto
    {
        public string Phone { get; set; } = null!;
        public string Otp { get; set; } = null!;
    }
}
