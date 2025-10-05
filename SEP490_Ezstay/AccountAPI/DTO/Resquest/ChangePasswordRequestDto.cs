namespace AccountAPI.DTO.Resquest
{
    public class ChangePasswordRequestDto
    {
        public string OldPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}
