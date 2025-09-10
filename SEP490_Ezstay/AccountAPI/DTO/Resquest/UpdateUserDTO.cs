using AccountAPI.Enums;

namespace AccountAPI.DTO.Resquest
{
    public class UpdateUserDTO
    {
        public string Address { get; set; }
        public GenderEnum Gender { get; set; }
        public string Bio { get; set; }
        public DateTime DateOfBirth { get; set; }

        // upload ảnh từ client
        public IFormFile Avatar { get; set; }

        public string? FullName { get; set; }
        public string? Phone { get; set; }
    }
}
