namespace AuthApi.DTO.Request
{
    public class ConfirmOtpRequest
    {
        public string Email { get; set; }
        public string Otp { get; set; }
    }
}
