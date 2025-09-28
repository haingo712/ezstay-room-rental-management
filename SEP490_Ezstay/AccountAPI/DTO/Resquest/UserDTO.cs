using AccountAPI.Enums;

namespace AccountAPI.DTO.Request
{
    public class UserDTO
    {
        
        public GenderEnum Gender { get; set; }
        public string Avatar { get; set; }
        public string Bio { get; set; }
        public string? ProvinceCode { get; set; }
        public string? CommuneCode { get; set; }
        public string? DetailAddress { get; set; }

    }
}
