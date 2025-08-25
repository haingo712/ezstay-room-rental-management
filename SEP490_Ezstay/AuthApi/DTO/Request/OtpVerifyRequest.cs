namespace AuthApi.DTO.Request
{
    public class OtpVerifyRequest
    {
        public string Email { get; set; } = null!;
        public string Otp { get; set; } = null!;
    }
}
