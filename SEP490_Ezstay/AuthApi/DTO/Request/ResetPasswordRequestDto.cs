namespace AuthApi.DTO.Request
{
    public class ResetPasswordRequestDto
    {
     
            public string Email { get; set; }        // Bổ sung email
            public string Token { get; set; }        // OTP từ email
            public string NewPassword { get; set; }  // Mật khẩu mới
        

     
    }
}
