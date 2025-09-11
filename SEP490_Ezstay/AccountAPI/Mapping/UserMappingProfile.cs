using AccountAPI.Data;
using AccountAPI.DTO.Reponse;
using AccountAPI.DTO.Request;
using AutoMapper;

namespace AccountAPI.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserDTO, User>();
            CreateMap<User, UserResponseDTO>(); 
        }
    }
}
