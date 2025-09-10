using AccountAPI.Enums;

namespace AccountAPI.DTO.Reponse
{
    public class UserResponseDTO
    {
        public Guid Id { get; set; }

        public string Adrress { get; set; }

        public GenderEnum Gender { get; set; }

        public string Avata { get; set; }

        public string Bio { get; set; }

        public DateTime DateOfBirth { get; set; }

        public Guid UserId { get; set; } // ID từ Account

        // Optional: Nếu bạn lấy thêm từ AccountService
        public string? FullName { get; set; }
        public string? Phone { get; set; }
    }
}
