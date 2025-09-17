using AuthApi.Enums;

namespace AuthApi.DTO.Request
{
    public class AccountRequest
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public RoleEnum Role { get; set; } 
    }
}
