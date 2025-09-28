using AccountAPI.Enums;

namespace AccountAPI.DTO.Response
{
    public class UserProfileResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public string FullName { get; set; }
        public string Phone { get; set; }

        public string? ProvinceCode { get; set; }
        public string? CommuneCode { get; set; }
        public string? DetailAddress { get; set; }
        public GenderEnum Gender { get; set; }
        public string Bio { get; set; }
        public DateTime DateOfBirth { get; set; }

        public string Avatar { get; set; }
    }
}
