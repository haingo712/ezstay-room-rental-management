
using AccountAPI.Model;
using AccountAPI.DTO.Reponse;
using AccountAPI.DTO.Request;
using AccountAPI.DTO.Resquest;
using AutoMapper;

namespace AccountAPI.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<CreateUserDTO, User>();
                 
            // Map User -> UserResponseDTO
            CreateMap<User, UserResponseDTO>();

            // Map UpdateUserDTO -> User
            CreateMap<UpdateUserDTO, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                // Allow Phone, Email, FullName to be updated from DTO
                .ForMember(dest => dest.ProvinceName, opt => opt.Ignore())
                .ForMember(dest => dest.WardName, opt => opt.Ignore())
                .ForMember(dest => dest.Avatar, opt => opt.Ignore())
                .ForMember(dest => dest.FrontImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.BackImageUrl, opt => opt.Ignore());
          }
    }
}
