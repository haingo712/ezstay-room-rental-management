using AccountAPI.Enums;

namespace AccountAPI.DTO.Request
{
    public class UserDTO
    {
        public string Adrress { get; set; }
        public GenderEnum Gender { get; set; }
        public string Avata { get; set; }
        public string Bio { get; set; }
       
    }
}
