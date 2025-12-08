using AuthApi.DTO.Request;
using AuthApi.DTO.Response;
using AuthApi.Models;
using AutoMapper;
using Shared.DTOs.Auths.Responses;

namespace AuthApi.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<SubmitOwnerRequestDto, OwnerRegistrationRequest>();
            CreateMap<OwnerRegistrationRequest, OwnerRequestResponseDto>();
        }
    }
}
