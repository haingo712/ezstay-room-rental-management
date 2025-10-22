using Shared.Enums;

namespace Shared.DTOs.Auths.Responses
{
    public class AccountResponse
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public DateTime CreateAt { get; set; }
        public RoleEnum Role { get; set; }
        public bool IsVerified { get; set; }
        public bool IsBanned { get; set; }

    }
}
