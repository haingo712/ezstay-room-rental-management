using AuthApi.DTO.Request;
using AuthApi.Models;
using AutoMapper;

namespace AuthApi.Mapping
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterRequestDto, Account>();
        }
    }
}
