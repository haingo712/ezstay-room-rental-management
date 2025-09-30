using AccountAPI.Enums;

namespace AccountAPI.DTO.Request
{
    public class UserDTO
    {

        public GenderEnum Gender { get; set; }
        public string Avatar { get; set; }
        public string Bio { get; set; }

        public string? Province { get; set; }
        public string? Commune { get; set; }
        public string? DetailAddress { get; set; }

        public DateTime? DateOfBirth { get; set; }

    }
}
