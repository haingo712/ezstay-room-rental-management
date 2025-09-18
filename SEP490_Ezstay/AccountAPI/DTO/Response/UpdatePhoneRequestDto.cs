namespace AccountAPI.DTO.Response
{
    public class UpdatePhoneRequestDto
    {
        public string Phone { get; set; } = string.Empty;
        public string Otp { get; set; } = string.Empty;
    }
}
