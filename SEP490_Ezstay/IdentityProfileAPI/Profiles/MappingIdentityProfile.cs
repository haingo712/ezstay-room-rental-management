using AutoMapper;
using IdentityProfileAPI.DTO.Requests;
using IdentityProfileAPI.DTO.Response;
using IdentityProfileAPI.Model;


namespace IdentityProfileAPI.Profiles;

public class MappingIdentityProfile: Profile
{
    public MappingIdentityProfile()
    {
        CreateMap<CreateIdentityProfileDto, IdentityProfile>();
        CreateMap<UpdateIdentityProfileDto, IdentityProfile>();
        CreateMap<IdentityProfile, IdentityProfileResponseDto>();
    }
}