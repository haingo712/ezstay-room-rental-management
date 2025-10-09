namespace AuthApi.DTO.Request
{
    public class ResetPasswordRequestDto
    {
        public string Email { get; set; } = null!;
        public string NewPassword { get; set; }
    }
}
